using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Pages
{
	public class RootPageViewModel : ViewModelBase
	{
		private readonly ICurrentModelProviderManager<IUserModel> _currentUserProvider;

		public RootPageViewModel(INavigationService navigationService, ICurrentModelProviderManager<IUserModel> currentUserProvider) : base(navigationService)
		{
			_currentUserProvider = currentUserProvider;
			ShowTasksCommand = new DelegateCommand(async ()=>await ShowTasksAsync());
			ShowWorkTimeStoreCommand = new DelegateCommand(async () => await ShowWorkTimeStoreAsync());
			ShowUserProfileCommand = new DelegateCommand(async () => await ShowUserProfileAsync());
			ShowSettingsCommand = new DelegateCommand(async () => await ShowSettingsAsync());
			LogoutCommand = new DelegateCommand(async () => await LogoutAsync());
		}

		public DelegateCommand ShowTasksCommand { get; }
		public DelegateCommand ShowWorkTimeStoreCommand { get; }
		public DelegateCommand ShowUserProfileCommand { get; }
		public DelegateCommand ShowSettingsCommand { get; }
		public DelegateCommand LogoutCommand { get; }

        private bool _isTaskBusy;
        public bool IsTaskBusy
        {
            get => _isTaskBusy;
            set
            {
                if (_isTaskBusy == value) return;
                _isTaskBusy = value;
                RaisePropertyChanged();
            }
        }

        private bool _isWorkBusy;
        public bool IsWorkBusy
        {
            get => _isWorkBusy;
            set
            {
                if (_isWorkBusy == value) return;
                _isWorkBusy = value;
                RaisePropertyChanged();
            }
        }

        private bool _isUserProfileBusy;
        public bool IsUserProfileBusy
        {
            get => _isUserProfileBusy;
            set
            {
                if (_isUserProfileBusy == value) return;
                _isUserProfileBusy = value;
                RaisePropertyChanged();
            }
        }

		private async Task LogoutAsync()
		{
			_currentUserProvider.SetItem(null);
			await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
		}

		private async Task ShowTasksAsync()
        {
            IsTaskBusy = true;
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/TaskGroupPage");
            IsTaskBusy = false;
        }

		private async Task ShowWorkTimeStoreAsync()
        {
            IsWorkBusy = true;
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/CompanyPage");
            IsWorkBusy = false;
        }

        private async Task ShowSettingsAsync()
		{
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/SettingsPage");
		}

		private async Task ShowUserProfileAsync()
        {
            IsUserProfileBusy = true;
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/UserDetailPage");
            IsUserProfileBusy = false;
        }
    }
}