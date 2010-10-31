using TodoApp.Models;
using Victoria.Test;

namespace TodoApp.Tests.Unit.Models {
    public class TodoItemTests {
        
        [Fact]
        public void should_set_id_on_save() {
            
            var item = new TodoItem();
            
            TodoItem.Add(item);
            TodoItem.SaveChanges();

            Assert.True(!string.IsNullOrEmpty(item.Id));
        }

        [Fact]
        public void should_not_overwrite_id() {
            
            var id = "id";
            var item = new TodoItem{Id = id};

            TodoItem.Add(item);
            TodoItem.SaveChanges();

            Assert.Equal(id,item.Id);
        }
    }
}