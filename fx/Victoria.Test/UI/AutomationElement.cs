using System;
using System.Threading;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Victoria.Test.UI {
    public class AutomationElement<T> where T : Control {
        private readonly T _wrapped;

        public AutomationElement(T wrapped) {
            _wrapped = wrapped;
        }

        public T Control { get { return _wrapped;}}

        private TextChangedEventHandler _waitForTextAction;
        public void WaitForText(Action<string> action) {

            var textbox = _wrapped as TextBox;
            _waitForTextAction = (s, e) => {
                textbox.TextChanged -= _waitForTextAction;
                action(textbox.Text);
                
            };
            textbox.TextChanged += _waitForTextAction;
        }

        public void Click() {
            var peer = new ButtonAutomationPeer(_wrapped as Button);
            var pattern = (IInvokeProvider)peer.GetPattern(PatternInterface.Invoke);
            pattern.Invoke();
        }

        public void SetText(string text) {
            var peer = new TextBoxAutomationPeer(_wrapped as TextBox);
            var pattern = (IValueProvider)peer.GetPattern(PatternInterface.Value);
            pattern.SetValue(text);
        }

        public string GetText() {

            var peer = new TextBoxAutomationPeer(_wrapped as TextBox);
            var pattern = (IValueProvider)peer.GetPattern(PatternInterface.Value);
            return pattern.Value;
        }
    }
}