using System;
using System.Windows.Navigation;
using Caliburn.Micro;

namespace Victoria.Test {
    public class NavigationServiceStub : INavigationService {
        
        public Uri NavigateUri;

        public bool Navigate(Uri source) {
            CurrentSource = source;
            Source = source;
            NavigateUri = source;
            return true;
        }

        public void StopLoading() {
        }

        public void GoBack() {
            throw new NotImplementedException();
        }

        public void GoForward() {
            throw new NotImplementedException();
        }

        public Uri Source {
            get; set; 
        }

        public bool CanGoBack {
            get { throw new NotImplementedException(); }
        }

        public bool CanGoForward {
            get { throw new NotImplementedException(); }
        }

        public Uri CurrentSource {
            get ; private set;
        }

        public event NavigatedEventHandler Navigated;
        public event NavigatingCancelEventHandler Navigating;
        
    }
}