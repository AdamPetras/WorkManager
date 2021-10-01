using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
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
		private readonly IPageDialogService _pageDialogService;
		private readonly ICurrentModelProvider<ITaskGroupModel> _currentTaskGroupProvider;
		private readonly IKanbanTaskGroupFacade _kanbanTaskGroupFacade;
		private readonly ITaskFacade _taskFacade;
		private readonly IToastMessageService _toastMessageService;
		private readonly DialogEventService _dialogEventService;

		public TaskKanbanPageViewModel(INavigationService navigationService, IDialogService dialogService, IPageDialogService pageDialogService,
			ICurrentModelProvider<ITaskGroupModel> currentTaskGroupProvider, IKanbanTaskGroupFacade kanbanTaskGroupFacade, ITaskFacade taskFacade, IToastMessageService toastMessageService,
			DialogEventService dialogEventService) : base(navigationService)
		{
			_dialogService = dialogService;
			_pageDialogService = pageDialogService;
			_currentTaskGroupProvider = currentTaskGroupProvider;
			_kanbanTaskGroupFacade = kanbanTaskGroupFacade;
			_taskFacade = taskFacade;
			_toastMessageService = toastMessageService;
			_dialogEventService = dialogEventService;
			BackCommand = new DelegateCommand<ITaskModel>(async (t) => await PushTaskBackAsync(t));
			CompleteCommand = new DelegateCommand<ITaskModel>(async (t) => await CompleteTaskAsync(t));
			DeleteCommand = new DelegateCommand<ITaskModel>(async (t) => await DeleteAsync(t));
			KanbanStateChangedCommand = new DelegateCommand<IKanbanStateModel>(async(t)=> await KanganStateChangedAsync(t));
			SelectTaskCommand = new DelegateCommand<ITaskModel>(OnSelectTask);
			EditCommand = new DelegateCommand(async () => await EditAsync(), () => SelectedTask != null && !IsDialogThrown);
			InitDialogCommands();
		}

		public DelegateCommand ClearWholeOrDeleteSingleTaskCommand { get; private set; }
		public DelegateCommand ShowAddTaskDialogCommand { get; private set; }
		public DelegateCommand<IKanbanStateModel> KanbanStateChangedCommand { get; }
		public DelegateCommand<ITaskModel> BackCommand { get; }
		public DelegateCommand<ITaskModel> CompleteCommand { get; }
		public DelegateCommand<ITaskModel> DeleteCommand { get; }
		public DelegateCommand<ITaskModel> SelectTaskCommand { get; }
		public DelegateCommand EditCommand { get; }


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

		private ITaskModel _selectedTask;
		public ITaskModel SelectedTask
		{
			get => _selectedTask;
			set
			{
				if (_selectedTask == value) return;
				_selectedTask = value;
				RaisePropertyChanged();
				EditCommand.RaiseCanExecuteChanged();
			}
		}

		protected override void InitializeInt()
		{
			base.InitializeInt();
			KanbanStates = new ObservableCollection<IKanbanStateModel>(_kanbanTaskGroupFacade.GetKanbansByTaskGroupId(_currentTaskGroupProvider.GetModel().Id).Select(s => s.Kanban));
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedFromInt(parameters);
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, Tasks);
		}

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= ShowAddTaskDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ClearWholeOrDeleteSingleTaskCommand.RaiseCanExecuteChanged;
		}

		private void InitDialogCommands()
		{
			ShowAddTaskDialogCommand = new DelegateCommand(async () => await ShowAddTaskDialogAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ShowAddTaskDialogCommand.RaiseCanExecuteChanged;
			ClearWholeOrDeleteSingleTaskCommand = new DelegateCommand(async () => await ClearWholeOrDeleteSingleTaskAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ClearWholeOrDeleteSingleTaskCommand.RaiseCanExecuteChanged;
		}

		private void UpdateButtonsVisibility(IKanbanStateModel kanbanState)
		{
			IsBackwardButtonVisible = kanbanState.StateOrder != 0;
			IsCompleteButtonVisible = _kanbanTaskGroupFacade.GetKanbansByTaskGroupId(_currentTaskGroupProvider.GetModel().Id).Max(s => s.Kanban.StateOrder) != kanbanState.StateOrder;
			IsDeleteButtonVisible = _kanbanTaskGroupFacade.GetKanbansByTaskGroupId(_currentTaskGroupProvider.GetModel().Id).Max(s => s.Kanban.StateOrder) == kanbanState.StateOrder;
		}

		private async Task NavigateToTaskDetailPageAsync(ITaskModel obj)
		{
		}

		private async Task PushTaskBackAsync(ITaskModel obj)
		{
			await MoveWithTask(obj, false);
		}

		private async Task CompleteTaskAsync(ITaskModel obj)
		{
			await MoveWithTask(obj, true);
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
				_toastMessageService.LongAlert(TranslateViewModelsSR.SwipeTaskOutside);
			}
		}

		private async Task ShowAddTaskDialogAsync()
		{
			IsDialogThrown = true;
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddTaskDialogView")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent,Tasks);
			IsDialogThrown = false;
		}

		private async Task ClearWholeOrDeleteSingleTaskAsync()
		{
			IsBusy = true;
			if (_selectedTask != null)
			{
				if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
					TranslateViewModelsSR.SelectedTaskDeleteMessageFormat(_selectedTask.Name),
					TranslateViewModelsSR.DialogYes, TranslateViewModelsSR.DialogNo))
				{
					await _taskFacade.RemoveAsync(_selectedTask.Id);
					Tasks.Remove(_selectedTask);
					_selectedTask = null;
				}
			}
			else
			{
				IsDialogThrown = true;
				if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning, TranslateViewModelsSR.TaskClearTasksMessage,
					TranslateViewModelsSR.DialogYes, TranslateViewModelsSR.DialogNo))
				{
					await _taskFacade.ClearAsync();
					Tasks.Clear();
				}
				IsDialogThrown = false;
			}
			IsBusy = false;
		}

		private async Task DeleteAsync(ITaskModel obj)
		{
			Tasks.Remove(obj);
			await _taskFacade.RemoveAsync(obj.Id);
		}

		private async Task KanganStateChangedAsync(IKanbanStateModel model)
		{
			IsBusy = true;
			if (model == null)
				return;
			SelectedKanbanState = model;
			Tasks = new ObservableCollection<ITaskModel>(await _taskFacade.GetTasksByTaskGroupIdAndKanbanStateAsync(_currentTaskGroupProvider.GetModel().Id, model.Name));
			UpdateButtonsVisibility(model);
			IsBusy = false;
		}

		private void OnSelectTask(ITaskModel obj)
		{
			SelectedTask = obj;
		}

		private async Task EditAsync()
		{
			IsBusy = true;
			await NavigationService.NavigateAsync("TaskDetailPage", new NavigationParameters() { { "Task", SelectedTask } });
		}
	}
}