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
		private readonly IEventAggregator _eventAggregator;

		public TaskDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ITaskFacade taskFacade, IEventAggregator eventAggregator) : base(navigationService)
		{
			_pageDialogService = pageDialogService;
			_taskFacade = taskFacade;
			_eventAggregator = eventAggregator;
			SaveCommand = new DelegateCommand(async() => await SaveAsync());
			DeleteTaskCommand = new DelegateCommand(async()=> await DeleteAsync());
		}

		public DelegateCommand SaveCommand { get; }
		public DelegateCommand DeleteTaskCommand { get; }


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
			base.OnNavigatedFromInt(parameters);
			SelectedTask =  new TaskModel(parameters.GetValue<ITaskModel>("Task"));	//vytváření nového modelu aby se neměnil model, který zde dojde pomocí navigace
		}

		private async Task SaveAsync()
		{
			await _taskFacade.UpdateAsync(SelectedTask);
			//_eventAggregator.GetEvent<AddTaskListApplicationEvent>().Publish(SelectedTask.State);
			await NavigationService.GoBackAsync(new NavigationParameters(){{ "DialogEvent", new UpdateAfterDialogCloseDialogEvent<ITaskModel>(SelectedTask)}});
		}

		private async Task DeleteAsync()
		{
			if (await _pageDialogService.DisplayAlertAsync(TaskKanbanPageViewModelSR.DeleteTaskTitle,TaskKanbanPageViewModelSR.DeleteTaskMessage,TaskKanbanPageViewModelSR.Yes,TaskKanbanPageViewModelSR.No))
			{
				await _taskFacade.RemoveAsync(SelectedTask.Id);
				//_eventAggregator.GetEvent<AddTaskListApplicationEvent>().Publish(SelectedTask.State);
				await NavigationService.GoBackAsync(new NavigationParameters(){{ "DialogEvent", new RemoveAfterDialogCloseDialogEvent<ITaskModel>(SelectedTask)}});
			}
		}
	}
}