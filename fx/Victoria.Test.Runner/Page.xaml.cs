using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Resources;

namespace Victoria.Test.Runner {
    public partial class Page : UserControl {
        public Page() {
            InitializeComponent();
        }

        public void MainPage_Loaded(object sender, RoutedEventArgs e) {
            HtmlPage.RegisterScriptableObject("runner",this);
        }

        [ScriptableMember]
        public int ExecuteTest(string testMethod) {
            return (new TestRunner(
                new TestMethodResolver(new TestAssemblyResolver(GetManifest())),
                new ConsoleOutputWriter(),
                new TestClassInstanceProvider())
                .Execute(testMethod)) ? 0 : 1;
        }

        private string GetManifest() {
            StreamResourceInfo manifest = Application.GetResourceStream(
                new Uri("AppManifest.xaml", UriKind.Relative));

            string content;
            using (var reader = new StreamReader(manifest.Stream)) {
                content = reader.ReadToEnd();
            }
            return content;
        }
    }
}
