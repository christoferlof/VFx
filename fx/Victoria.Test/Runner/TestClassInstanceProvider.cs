using System;
using Microsoft.Phone.Controls;

namespace Victoria.Test.Runner {
    public interface ITestClassInstanceProvider {
        object CreateInstance(Type testClass);
    }


    public class TestClassInstanceProvider : ITestClassInstanceProvider {
        public object CreateInstance(Type testClass) {
            return Activator.CreateInstance(testClass);
        }
    }

// ReSharper disable InconsistentNaming
    public class UITestClassInstanceProvider : ITestClassInstanceProvider {
// ReSharper restore InconsistentNaming
        private readonly PhoneApplicationFrame _rootFrame;

        public UITestClassInstanceProvider(PhoneApplicationFrame rootFrame) {
            _rootFrame = rootFrame;
        }

        public object CreateInstance(Type testClass) {
            var testObject = Activator.CreateInstance(testClass);

            if (ShouldAttachRootVisual(testObject)) {
                ((IntegrationTest)testObject).RootFrame = _rootFrame;
            }
            return testObject;
        }

        private bool ShouldAttachRootVisual(object testObject) {
            return testObject is IntegrationTest && _rootFrame != null;
        }
    }
}