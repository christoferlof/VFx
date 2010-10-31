using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Victoria.Test.Runner;

namespace Victoria.Test.UI {
    public class AutomationPage {

        private readonly string _page;
        private readonly PhoneApplicationFrame _applicationFrame;
        private readonly WorkItem _workItem;

        private PhoneApplicationFrame Frame {
            get { return _applicationFrame; }
        }

        public PhoneApplicationPage Page {
            get { return Frame.Content as PhoneApplicationPage; }
        }
      
        public AutomationPage(string page, PhoneApplicationFrame applicationFrame, WorkItem workItem) {
            _page               = page;
            _applicationFrame   = applicationFrame;
            _workItem           = workItem;
        }
      
        public AutomationElement<T> Find<T>(string controlName) where T : Control {
            return new AutomationElement<T>((T)Page.FindName(controlName));
        }

        public AutomationApplicationBar ApplicationBar {
            get { return new AutomationApplicationBar(Page); }
        }

        private NavigatedEventHandler _readyAction;

        public void Ready(Action<AutomationPage> action) {
            Frame.Dispatcher.BeginInvoke(() => {
                _readyAction = (s, e) => {
                    Frame.Navigated -= _readyAction;
                    _workItem.ExecuteTest(() => action(this));
                };
                Frame.Navigated += _readyAction;
                Frame.Navigate(new Uri(_page + "?r=" + DateTime.UtcNow.Ticks, UriKind.Relative));
            });
        }
    }
}