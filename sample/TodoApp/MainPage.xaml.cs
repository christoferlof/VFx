using System.Threading;
using System.Windows;
using Microsoft.Phone.Controls;
using Victoria.Test.Runner;

namespace TodoApp {
    public partial class MainPage : PhoneApplicationPage {
        
        public MainPage() {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
#if INTEGRATIONTEST

            var runner = new TestRunner(
                new TestMethodResolver(
                    new StaticAssemblyResolver(GetType())),
                    new DebugOutputWriter(),
                    new UITestClassInstanceProvider((PhoneApplicationFrame)Application.Current.RootVisual)
            );
            
            ThreadPool.QueueUserWorkItem((d) => runner.Execute(string.Empty)); //execute all tests
#endif
        }
    }
}