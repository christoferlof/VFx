using System;
using System.Threading;
using Microsoft.Phone.Controls;
using Victoria.Test.Runner;
using Victoria.Test.UI;

namespace Victoria.Test {
    public abstract class IntegrationTest {
        
        public PhoneApplicationFrame RootFrame { get; set; }

        public WorkItem WorkItem { get; set; }

        protected AutomationPage Page(string viewUri) {
            return new AutomationPage(viewUri,RootFrame, WorkItem);
        }
    }
}