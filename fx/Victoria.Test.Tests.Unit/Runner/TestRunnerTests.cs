using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Victoria.Test.Exceptions;
using Victoria.Test.Runner;

namespace Victoria.Test.Tests.Unit.Runner {
    public class TestRunnerTests {
    
        FakeMethodResolver _methodResolver  = new FakeMethodResolver();
        string _failingMethod               = "this_is_a_failing_test_for_testing";
        string _testClass                   = "Victoria.Test.Tests.Unit.Runner.TestRunnerTests";
        FakeOutputWriter _outputWriter      = new FakeOutputWriter();

        [Fact]
        public void should_return_success_when_all_tests_pass() {
            var methodResolver = new TestMethodResolver(new FakeAssemblyResolver());
            var runner = new TestRunner(methodResolver,_outputWriter, new TestClassInstanceProvider());
            var testMethod = "Victoria.Test.Tests.Unit.AssertTests.TrueThrows";
            var result = runner.Execute(testMethod);

            Assert.True(result);
        }

        private TestRunner CreateRunner() {
            return new TestRunner(_methodResolver,_outputWriter, new TestClassInstanceProvider());
        }

        private bool ExecuteFailingRunner() {
            var runner = CreateRunner();
            var testPath = _testClass + "." + _failingMethod;
            return runner.Execute(testPath);
        }

        [Fact]
        public void should_return_failure_when_a_single_test_fails() {
            var result = ExecuteFailingRunner();
            Assert.True(!result);
        }

        [Fact]
        public void should_output_executing_test_case() {
            ExecuteFailingRunner();
            Assert.True(_outputWriter.Output.Where(x => x.Contains("this_is_a_failing_test_for_testing")).Any());
        }

        [Fact]
        public void should_output_testrun_result() {
            ExecuteFailingRunner();
            Assert.True(_outputWriter.Output.Where(x => x.Contains("Testrun failed")).Any());
        }

        #region fakes

        public void this_is_a_failing_test_for_testing() {
            throw new TrueException();
        }

        public class FakeAssemblyResolver : TestAssemblyResolver {

            public FakeAssemblyResolver() : this(string.Empty) {}
            
            public FakeAssemblyResolver(string manifest) : base(manifest) {}

            public override IEnumerable<string> GetTestAssemblies() {
                return new []{GetType().Assembly.FullName};
            }
        }

        public class FakeMethodResolver : TestMethodResolver {
            
            public FakeMethodResolver() : this(new FakeAssemblyResolver()){}
            
            public FakeMethodResolver(TestAssemblyResolver testAssemblyResolver) 
                : base(testAssemblyResolver) {
            }

            public override MemberInfo GetTestMethod(string testPath) {
                return typeof(TestRunnerTests)
                    .FindMembers(MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, null, null)
                    .Where(n => n.Name == "this_is_a_failing_test_for_testing")
                    .SingleOrDefault();
            }
        }

        public class FakeOutputWriter : OutputWriter{
            
            public IList<string> Output = new List<string>();
            
            public override void Write(string message) {
                Output.Add(message);
            }
        }

        #endregion
    }

    
}