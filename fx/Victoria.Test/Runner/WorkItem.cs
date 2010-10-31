using System;
using System.Reflection;
using System.Threading;
using Victoria.Test.Exceptions;

namespace Victoria.Test.Runner {
    public class WorkItem {
        
        public static void Enqueue(MemberInfo method, 
            ITestClassInstanceProvider instanceProvider, EventHandler<EventArgs> onCompleteCallback) {

            ThreadPool.QueueUserWorkItem((d) => {
                var workItem = new WorkItem(method, instanceProvider);
                workItem.Complete += onCompleteCallback;
                workItem.Run();
            });
        }

        private readonly ITestClassInstanceProvider _instanceProvider;

        public WorkItem(MemberInfo testMethod, ITestClassInstanceProvider instanceProvider) {
            
            TestMethod = testMethod;
            _instanceProvider = instanceProvider;
        }

        public MemberInfo TestMethod {get; private set;}

        public void Run() {
            ExecuteMethod(TestMethod);
        }

        private void ExecuteMethod(MemberInfo method) {

            var testObject = _instanceProvider.CreateInstance(method.DeclaringType);

            if (!(testObject is IntegrationTest)) {
                ExecuteTest(() => InvokeTestMethod(method, testObject));
            } else {
                ((IntegrationTest)testObject).WorkItem = this; //let AutomationPage call ExecuteTest
                InvokeTestMethod(method, testObject);
            }
        }

        protected void InvokeTestMethod(MemberInfo method, object testClass) {

            testClass.GetType().InvokeMember(
                method.Name,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                null,
                testClass,
                null
            );
        }

        public void ExecuteTest(Action test) {
            try {
                test();
                Pass();
            } catch (Exception ex) {
                Fail(ex);
            } finally { 
                OnComplete(this, EventArgs.Empty);
            }
        }

        public bool Result { get; private set; }

        public string ResultMessage {get; private set;}

        private void Pass() {
            Result = true;
            ResultMessage = FormatRestultMessage(TestMethod,Result,string.Empty);
        }

        private void Fail(Exception exception) {
            Result = false;
            ResultMessage = FormatRestultMessage(TestMethod,Result,FormatFailedMessage(exception));
        }

        public event EventHandler<EventArgs> Complete;

        protected virtual void OnComplete(object sender, EventArgs eventArgs) {
            if (Complete != null) Complete(sender, eventArgs);
        }

        private static string FormatRestultMessage(MemberInfo method, bool testmethodPass, string failedMessage) {
            return string.Format("{0} {1}.{2} {3}",
                                 (testmethodPass) ? "Passed" : "Failed",
                                 method.DeclaringType.Name,
                                 method.Name,
                                 failedMessage);
        }

        private static string FormatFailedMessage(Exception ex) {
            string failedMessage;
            var exception = GetException(ex);
            if (exception is AssertException) {
                failedMessage = string.Format("=> {0}", exception.Message);
            } else {
                failedMessage = string.Format("=> {0}: {1}", exception.GetType().Name,
                                              exception.Message);
            }
            return failedMessage;
        }

        private static Exception GetException(Exception ex) {
            return ex.InnerException ?? ex;
        }
    }
}