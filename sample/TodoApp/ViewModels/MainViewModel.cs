using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using TodoApp.Models;

namespace TodoApp.ViewModels {
    public class MainViewModel {
        
        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<TodoItem> Items {
            get {
                return TodoItem.All;
            }
        }

    }
}