using System;
using System.Reflection;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Victoria.Test.UI {
    public class AutomationApplicationBarIconButton {
        private readonly ApplicationBarIconButton _button;
        private readonly PhoneApplicationPage _page;

        public AutomationApplicationBarIconButton(ApplicationBarIconButton button, PhoneApplicationPage page) {
            _button = button;
            _page = page;
        }

        public void Click() {
            var handlerName = string.Format("{0}_Click", _button.Text);
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod;
            var handler = _page.GetType().GetMethod(handlerName, flags);
            handler.Invoke(_page, new object[] { _button, EventArgs.Empty });
        }
    }
}