using Prism.Navigation;

namespace WorkManager.ViewModels.BaseClasses
{
    public class CollectionViewModelBase : ViewModelBase
    {
        public CollectionViewModelBase(INavigationService navigationService) : base(navigationService)
        {
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            private set
            {
                if (_isRefreshing == value) return;
                _isRefreshing = value;
                RaisePropertyChanged();
            }
        }

        protected void BeginRefresh() => IsRefreshing = true;
        protected void EndRefresh() => IsRefreshing = false;
    }
}