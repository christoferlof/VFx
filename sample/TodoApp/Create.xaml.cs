using Microsoft.Phone.Controls;

namespace TodoApp {
    public partial class Create : PhoneApplicationPage {
        public Create() {
            InitializeComponent();
        }

        private void CreateTodo_Click(object sender, System.Windows.RoutedEventArgs e) {
            ((CreateViewModel)DataContext).CreateTodo();
        }
    }
}