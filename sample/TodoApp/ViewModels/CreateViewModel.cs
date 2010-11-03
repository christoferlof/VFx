using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TodoApp.Models;

namespace TodoApp.ViewModels {
    public class CreateViewModel : INotifyPropertyChanged {
        
        private string _id;
        public string Id {
            get { return _id; }
            set {
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        private string _title;
        public string Title {
            get { return _title; }
            set {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public void CreateTodo() {
            var todo = new TodoItem { Title = Title };

            TodoItem.Add(todo);
            TodoItem.SaveChanges();

            Id = todo.Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
