using TodoApp.Models;
using TodoApp.ViewModels;
using Victoria.Test;

namespace TodoApp.Tests.Unit.ViewModels {
    public class MainViewModelTests {

        public MainViewModelTests() {
            TodoItem.Clear();
            TodoItem.SaveChanges();
        }

        [Fact]
        public void should_display_all_todos() {
            
            TodoItem.Add(new TodoItem());
            TodoItem.Add(new TodoItem());
            TodoItem.SaveChanges();
            
            var viewModel = new MainViewModel();

            Assert.Equal(2,viewModel.Items.Count);

        }
    }
}