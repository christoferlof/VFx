using System;
using System.Runtime.Serialization;
using Victoria.Data;

namespace TodoApp.Models {
    
    public class TodoItem : ActiveRecord<TodoItem> {

        public string Id {get;set;}

        public string Title {get;set;}

        public override void OnSaveChanges() {
            if (string.IsNullOrEmpty(Id)) { 
                Id = Guid.NewGuid().ToString();
            }
        }
    }
}
