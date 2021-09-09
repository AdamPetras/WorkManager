using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Views
{
	public class WorkRecordEmptyViewModel: ViewModelBase
	{
		private readonly IDialogService _dialogService;

		public WorkRecordEmptyViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService)
		{
			_dialogService = dialogService;
			ShowAddWorkRecordCommand = new DelegateCommand(ShowAddWorkRecord);
		}

		public DelegateCommand ShowAddWorkRecordCommand { get; }

		private void ShowAddWorkRecord()
		{
			_dialogService.ShowDialog("AddWorkRecordDialogView");
		}
	}
}