using System.Linq;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Victoria.Test.UI {
    public class AutomationApplicationBar {
        private readonly PhoneApplicationPage _page;

        public AutomationApplicationBar(PhoneApplicationPage page) {
            _page = page;
        }

        public AutomationApplicationBarIconButton Button(string buttonText) {
            var button = _page.ApplicationBar.Buttons
                .Cast<ApplicationBarIconButton>()
                .Where(x => x.Text == buttonText)
                .FirstOrDefault();
            return new AutomationApplicationBarIconButton(button,_page);
        }
    }
}