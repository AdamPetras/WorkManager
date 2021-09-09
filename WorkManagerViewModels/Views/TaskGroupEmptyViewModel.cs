using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Views
{
	public class TaskGroupEmptyViewModel : ViewModelBase
	{
		private readonly IDialogService _dialogService;

		public TaskGroupEmptyViewModel(INavigationService navigationService, IDialogService dialogService) : base(navigationService)
		{
			_dialogService = dialogService;
			ShowAddTaskGroupCommand = new DelegateCommand(ShowAddTaskGroup);
		}

		public DelegateCommand ShowAddTaskGroupCommand { get; }

		private void ShowAddTaskGroup()
		{
			_dialogService.ShowDialog("AddTaskGroupDialogView");
		}
	}
}