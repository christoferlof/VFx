using Caliburn.Micro;
using TodoApp.Models;

namespace TodoApp {
    public class CreateViewModel : Screen {
        public string Title { get; set; }

        private string _id;
        public string Id {
            get { return _id; }
            set {
                _id = value;
                NotifyOfPropertyChange(() => Id);
            }
        }

        public void CreateTodo() {

            var todo = new TodoItem {
                Title = Title
            };

            TodoItem.Add(todo);
            TodoItem.SaveChanges();

            Id = todo.Id;
        }

    }
}