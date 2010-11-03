using Microsoft.Phone.Controls;
using TodoApp.ViewModels;

namespace TodoApp {
    public partial class MainPage : PhoneApplicationPage {

        public MainPage() {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}