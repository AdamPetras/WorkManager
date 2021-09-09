using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Pages
{
	public class RootPageViewModel : ViewModelBase
	{
		public RootPageViewModel(INavigationService navigationService) : base(navigationService)
		{
			ShowTasksCommand = new DelegateCommand(async ()=>await ShowTasksAsync());
			ShowWorkTimeStoreCommand = new DelegateCommand(async () => await ShowWorkTimeStoreAsync());
		}

		public DelegateCommand ShowTasksCommand { get; }

		public DelegateCommand ShowWorkTimeStoreCommand { get; }

		private async Task ShowTasksAsync()
		{
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/TaskGroupPage");
		}

		private async Task ShowWorkTimeStoreAsync()
		{
			await NavigationService.NavigateAsync("/RootPage/NavigationPage/CompanyPage");
		}
	}
}