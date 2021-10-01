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

		private async Task LogoutAsync()
		{
			_currentUserProvider.SetItem(null);
			await NavigationService.NavigateAsync("/NavigationPage/LoginPage");
		}

		private async Task ShowTasksAsync()
		{
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/TaskGroupPage");
		}

		private async Task ShowWorkTimeStoreAsync()
		{
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/CompanyPage");
		}

		private async Task ShowSettingsAsync()
		{
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/SettingsPage");
		}

		private async Task ShowUserProfileAsync()
		{
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/UserDetailPage");
		}
	}
}