using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Victoria.Test.Console {
    class Program {

        private static string _testMethods;
        private const string RunnerPage = "/Content/RunnerPage.html";

        [STAThread]
        private static void Main(string[] args) {
            Packager.CreatePackage();
            ExecuteTests(args);
        }

        private static void ExecuteTests(string[] args) {
            if (args != null) {
                _testMethods = string.Join("|", args);
            }

            var browser = new WebBrowser();
            browser.DocumentCompleted += DocumentCompleted;
            var url = Path.GetDirectoryName(typeof(Program).Assembly.CodeBase) + RunnerPage;
            browser.Navigate(url);
            Application.Run();
        }

        private static void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
            if (!e.Url.AbsoluteUri.EndsWith(RunnerPage)) return;
            
            Application.DoEvents();
            var browser = (WebBrowser) sender;
            var testResult = browser.Document.InvokeScript("executeTest", new[] {_testMethods});
            Application.Exit();
            Environment.ExitCode = int.Parse(testResult.ToString());
        }
    }
}
