using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.Services;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class TaskKanbanPageViewModel : ViewModelBase
	{
		private readonly IDialogService _dialogService;
		private readonly IEventAggregator _eventAggregator;
		private readonly IPageDialogService _pageDialogService;
		private readonly ICurrentModelProvider<ITaskGroupModel> _currentTaskGroupProvider;
		private readonly IKanbanTaskGroupFacade _kanbanTaskGroupFacade;
		private readonly ITaskFacade _taskFacade;
		private readonly IToastMessageService _toastMessageService;
		private readonly DialogEventService _dialogEventService;

		public TaskKanbanPageViewModel(INavigationService navigationService, IDialogService dialogService, IEventAggregator eventAggregator, IPageDialogService pageDialogService,
			ICurrentModelProvider<ITaskGroupModel> currentTaskGroupProvider, IKanbanTaskGroupFacade kanbanTaskGroupFacade, ITaskFacade taskFacade, IToastMessageService toastMessageService,
			DialogEventService dialogEventService) : base(navigationService)
		{
			_dialogService = dialogService;
			_eventAggregator = eventAggregator;
			_pageDialogService = pageDialogService;
			_currentTaskGroupProvider = currentTaskGroupProvider;
			_kanbanTaskGroupFacade = kanbanTaskGroupFacade;
			_taskFacade = taskFacade;
			_toastMessageService = toastMessageService;
			_dialogEventService = dialogEventService;
			KanbanStates = new ObservableCollection<IKanbanStateModel>(_kanbanTaskGroupFacade.GetKanbansByTaskGroupId(currentTaskGroupProvider.GetModel().Id).Select(s=>s.Kanban));
			NavigateToTaskDetailPageCommand = new DelegateCommand<ITaskModel>(async (t) => await NavigateToTaskDetailPageAsync(t));
			ShowAddTaskDialogCommand = new DelegateCommand(async()=> await ShowAddTaskDialogAsync());
			BackCommand = new DelegateCommand<ITaskModel>(async (t) => await PushTaskBackAsync(t));
			CompleteCommand = new DelegateCommand<ITaskModel>(async (t) => await CompleteTaskAsync(t));
			ClearWholeTasksCommand = new DelegateCommand(async () => await ClearWholeTasks());
			DeleteCommand = new DelegateCommand<ITaskModel>(async (t) => await DeleteAsync(t));
			FilterTasksCommand = new DelegateCommand<IKanbanStateModel>(FilterTasksAsync);
		}

		public DelegateCommand ClearWholeTasksCommand { get; }
		public DelegateCommand<ITaskModel> NavigateToTaskDetailPageCommand { get; }
		public DelegateCommand ShowAddTaskDialogCommand { get; }
		public DelegateCommand<IKanbanStateModel> FilterTasksCommand { get; }
		public DelegateCommand<ITaskModel> BackCommand { get; }
		public DelegateCommand<ITaskModel> CompleteCommand { get; }
		public DelegateCommand<ITaskModel> DeleteCommand { get; }

		private IKanbanStateModel _selectedKanbanState;
		public IKanbanStateModel SelectedKanbanState
		{
			get => _selectedKanbanState;
			set
			{
				if (_selectedKanbanState == value) return;
				_selectedKanbanState = value;
				RaisePropertyChanged();
			}
		}

		private ObservableCollection<IKanbanStateModel> _kanbanStates;
		public ObservableCollection<IKanbanStateModel> KanbanStates
		{
			get => _kanbanStates;
			set
			{
				if (_kanbanStates == value) return;
				_kanbanStates = value;
				RaisePropertyChanged();
			}
		}

		private ObservableCollection<ITaskModel> _tasks;
		public ObservableCollection<ITaskModel> Tasks
		{
			get => _tasks;
			set
			{
				if (_tasks == value) return;
				_tasks = value;
				RaisePropertyChanged();
			}
		}

		private bool _isBackwardButtonVisible;
		public bool IsBackwardButtonVisible
		{
			get => _isBackwardButtonVisible;
			set
			{
				if (_isBackwardButtonVisible == value) return;
				_isBackwardButtonVisible = value;
				RaisePropertyChanged();
			}
		}

		private bool _isCompleteButtonVisible;
		public bool IsCompleteButtonVisible
		{
			get => _isCompleteButtonVisible;
			set
			{
				if (_isCompleteButtonVisible == value) return;
				_isCompleteButtonVisible = value;
				RaisePropertyChanged();
			}
		}

		private bool _isDeleteButtonVisible;
		public bool IsDeleteButtonVisible
		{
			get => _isDeleteButtonVisible;
			set
			{
				if (_isDeleteButtonVisible == value) return;
				_isDeleteButtonVisible = value;
				RaisePropertyChanged();
			}
		}

		private void UpdateButtonsVisibility(IKanbanStateModel kanbanState)
		{
			IsBackwardButtonVisible = kanbanState.StateOrder != 0;
			IsCompleteButtonVisible = _kanbanTaskGroupFacade.GetKanbansByTaskGroupId(_currentTaskGroupProvider.GetModel().Id).Max(s => s.Kanban.StateOrder) != kanbanState.StateOrder;
			IsDeleteButtonVisible = _kanbanTaskGroupFacade.GetKanbansByTaskGroupId(_currentTaskGroupProvider.GetModel().Id).Max(s => s.Kanban.StateOrder) == kanbanState.StateOrder;
		}

		private async Task NavigateToTaskDetailPageAsync(ITaskModel obj)
		{
			await NavigationService.NavigateAsync("TaskDetailPage", new NavigationParameters() { { "Task", obj } });
		}

		private async Task PushTaskBackAsync(ITaskModel obj)
		{
			MoveWithTask(obj, false);
		}

		private async Task CompleteTaskAsync(ITaskModel obj)
		{
			MoveWithTask(obj, true);
		}

		private async Task MoveWithTask(ITaskModel obj, bool isMoveToComplete)
		{
			IKanbanStateModel model = (await _kanbanTaskGroupFacade
				.GetKanbansByTaskGroupIdAsync(_currentTaskGroupProvider.GetModel().Id))
				.FirstOrDefault(s => s.Kanban.StateOrder == SelectedKanbanState.StateOrder + (isMoveToComplete ? +1 : -1))?.Kanban;
			if (model != null)
			{
				Tasks.Remove(obj);
				obj.State = model;
				await _taskFacade.UpdateAsync(obj);
			}
			else
			{
				_toastMessageService.LongAlert(TaskKanbanPageViewModelSR.SwipeTaskOutside);
			}
		}

		private async Task ShowAddTaskDialogAsync()
		{
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddTaskDialogView")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent,Tasks);
		}

		private async Task ClearWholeTasks()
		{
			if (await _pageDialogService.DisplayAlertAsync(TaskKanbanPageViewModelSR.ClearTasksMessageTitle, TaskKanbanPageViewModelSR.ClearTasksMessage,
				TaskKanbanPageViewModelSR.Yes, TaskKanbanPageViewModelSR.No))
			{
				foreach (ITaskModel taskModel in Tasks)
				{
					//nepotřebu await nechci čekat může běžet nezávisle a exceptiony by neměly vyskočit pokud všechno funguje jak má
					_taskFacade.RemoveAsync(taskModel.Id);
				}
				Tasks.Clear();
			}
		}

		private async Task DeleteAsync(ITaskModel obj)
		{
			Tasks.Remove(obj);
			_taskFacade.RemoveAsync(obj.Id);
		}

		private void FilterTasksAsync(IKanbanStateModel model)
		{
			if (model == null)
				return;
			SelectedKanbanState = model;
			Tasks = new ObservableCollection<ITaskModel>(_taskFacade.GetTasksByTaskGroupIdAndKanbanState(_currentTaskGroupProvider.GetModel().Id, model.Name));
			UpdateButtonsVisibility(model);
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedFromInt(parameters);
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent,Tasks);
		}
	}
}