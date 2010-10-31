using Victoria.Test;

namespace TodoApp.Tests.Unit.ViewModels {
    public class CreateViewModelTests {
        
        [Fact]
        public void should_display_id_of_created_todo() {
            var viewModel = new CreateViewModel();

            viewModel.Title = "title";
            viewModel.CreateTodo();

            Assert.True(!string.IsNullOrEmpty(viewModel.Id));
        }

    }
}