using Prism.Navigation;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Pages
{
	public class SettingsPageViewModel : ViewModelBase
	{
		public SettingsPageViewModel(INavigationService navigationService, ViewModelTaskExecute viewModelTaskExecute) : base(navigationService, viewModelTaskExecute)
		{
		}
	}
}