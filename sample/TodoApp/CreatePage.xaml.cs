using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using TodoApp.ViewModels;

namespace TodoApp {
    public partial class CreatePage : PhoneApplicationPage {
        public CreatePage() {
            InitializeComponent();
            DataContext = new CreateViewModel();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e) {
            ((CreateViewModel)DataContext).CreateTodo();
        }
    }
}