using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Views
{
	public class CompanyEmptyViewModel:ViewModelBase
	{
		private readonly IDialogService _dialogService;

		public CompanyEmptyViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService)
		{
			_dialogService = dialogService;
			ShowAddCompanyCommand = new DelegateCommand(ShowAddCompany);
		}

		public DelegateCommand ShowAddCompanyCommand { get; }

		private void ShowAddCompany()
		{
			_dialogService.ShowDialog("AddCompanyDialogView");
		}
	}
}