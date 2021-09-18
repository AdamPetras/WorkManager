using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class TaskDetailPageViewModel : ViewModelBase
	{
		private readonly IPageDialogService _pageDialogService;
		private readonly ITaskFacade _taskFacade;

		public TaskDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ITaskFacade taskFacade) : base(navigationService)
		{
			_pageDialogService = pageDialogService;
			_taskFacade = taskFacade;
			SaveCommand = new DelegateCommand(async() => await SaveAsync());
			InitDialogCommands();
		}

		public DelegateCommand SaveCommand { get; }
		public DelegateCommand DeleteTaskCommand { get; private set; }


		private ITaskModel _selectedTask;
		public ITaskModel SelectedTask
		{
			get => _selectedTask;
			private set
			{
				if (_selectedTask == value) return;
				_selectedTask = value;
				RaisePropertyChanged();
			}
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			SelectedTask = new TaskModel(parameters.GetValue<ITaskModel>("Task"));	//vytváření nového modelu aby se neměnil model, který zde dojde pomocí navigace
		}

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= DeleteTaskCommand.RaiseCanExecuteChanged;
		}

		private void InitDialogCommands()
		{
			DeleteTaskCommand = new DelegateCommand(async () => await DeleteAsync(), () => !IsDialogThrown);
			DialogThrownEvent += DeleteTaskCommand.RaiseCanExecuteChanged;
		}

		private async Task SaveAsync()
		{
			await _taskFacade.UpdateAsync(SelectedTask);
			await NavigationService.GoBackAsync(new NavigationParameters(){{ "DialogEvent", new UpdateAfterDialogCloseDialogEvent<ITaskModel>(SelectedTask)}});
		}

		private async Task DeleteAsync()
		{
			IsDialogThrown = true;
			if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.TaskDeleteTitle, TranslateViewModelsSR.TaskDeleteMessage, TranslateViewModelsSR.DialogYes, TranslateViewModelsSR.DialogNo))
			{
				await _taskFacade.RemoveAsync(SelectedTask.Id);
				await NavigationService.GoBackAsync(new NavigationParameters(){{ "DialogEvent", new RemoveAfterDialogCloseDialogEvent<ITaskModel>(SelectedTask)}});
			}
			IsDialogThrown = false;
		}
	}
}