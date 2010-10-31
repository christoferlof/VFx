using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Phone.Controls;
using Victoria.Test.Exceptions;

namespace Victoria.Test.Runner {
    public class TestRunner {

        private readonly TestMethodResolver _testMethodResolver;
        private readonly OutputWriter _outputWriter;
        private readonly ITestClassInstanceProvider _instanceProvider;

        private int _passedCounter;
        private int _failedCounter;

        private readonly ManualResetEvent _waitHandle = new ManualResetEvent(false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="testMethodResolver"></param>
        /// <param name="outputWriter"></param>
        /// <param name="instanceProvider"></param>
        public TestRunner(TestMethodResolver testMethodResolver, OutputWriter outputWriter, 
            ITestClassInstanceProvider instanceProvider) {
            
            _testMethodResolver = testMethodResolver;
            _outputWriter = outputWriter;
            _instanceProvider = instanceProvider;
        }

        /// <summary>
        /// Executes the suite of tests
        /// </summary>
        /// <param name="testPath">The path of the test(s) to execute.</param>
        /// <returns>True if the testrun succeeded; otherwise false.</returns>
        /// <remarks>
        /// Supported test paths are
        ///   - Full path to single testmethod. Will execute single testmethod. name.space.Tests.Unit.testmethod
        ///   - Root namespace. Will execute all tests within that project. 
        ///     By convention the project namespace must end with "Tests.Unit". name.space.Tests.Unit
        ///   - Empty. Will execute all testmethods found in the assemblies that are returned by the TestAssemblyResolver.
        /// </remarks>
        public bool Execute(string testPath) {
            try {

                _testMethodResolver.LoadTestAssemblies();

                var methods = _testMethodResolver.GetTestMethods(testPath);
                if (!methods.Any()) return ExitRun(false, "Couldn't find any matching test methods");
                
                foreach(var method in methods){                    
                    _waitHandle.Reset();
                    WorkItem.Enqueue(method,_instanceProvider,OnWorkItemComplete);
                    _waitHandle.WaitOne();
                }
                
                return ExitRun(TestRunResult(), string.Empty);
            } catch (Exception ex) {
                var msg = string.Format("Catastrophic failure! => {0}",ex);
                return ExitRun(false,msg);
            }

        }

        void OnWorkItemComplete(object sender, EventArgs e) {

            try {
                var workItem = (WorkItem)sender;
                IncrementResult(workItem.Result);
                _outputWriter.Write(workItem.ResultMessage);
                _waitHandle.Set();
            } catch (Exception ex) {
                _outputWriter.Write(ex.ToString());
            }
        }

        private void IncrementResult(bool result) {
            if (result) {
                _passedCounter++;
            } else {
                _failedCounter++;
            }
        }

        private bool TestRunResult() {
            return _failedCounter == 0;
        }


        private bool ExitRun(bool testrunPass, string message) {
            var testrunMessage = string.Format("\nTestrun {0}. {1}", (testrunPass) ? "succeeded" : "failed", message);
            _outputWriter.Write(testrunMessage);
            _outputWriter.Write(string.Format("{0} tests passed", _passedCounter));
            _outputWriter.Write(string.Format("{0} tests failed", _failedCounter));
            return testrunPass;
        }
    }
}