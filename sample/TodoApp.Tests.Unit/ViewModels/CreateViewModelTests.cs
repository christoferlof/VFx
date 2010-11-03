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
using TodoApp.ViewModels;
using Victoria.Test;

namespace TodoApp.Tests.Unit.ViewModels {
    public class CreateViewModelTests {

        [Fact]
        public void should_fire_property_changed() {
            
            var viewModel = new CreateViewModel();
            var fired = false;
            viewModel.PropertyChanged += (s,e) => fired = true;
 
            viewModel.Id = "Id";

            Assert.True(fired);
        }

        [Fact]
        public void should_display_id_of_newly_created_todo() {
            
            var viewModel = new CreateViewModel();

            viewModel.CreateTodo();

            Assert.True(!string.IsNullOrEmpty(viewModel.Id));
        }
    }
}
