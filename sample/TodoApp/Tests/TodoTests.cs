using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Victoria.Test;

namespace TodoApp.Tests {
    public class TodoTests : IntegrationTest {
        
        [Fact]
        public void should_create_todo() {
            Page("/Create.xaml").Ready(page => {
                page.Find<TextBox>("TitleTextBox").SetText("New Todo");
                page.Find<Button>("CreateTodoButton").Click();
                page.Find<TextBox>("IdTextBox").WaitForText(text =>
                Assert.True(!string.IsNullOrEmpty(text)) ); 
            });
        }
    }
}
