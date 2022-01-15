using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using WorkManager.BL.NavigationParams;
using WorkManager.BL.Services;
using WorkManager.Extensions;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
    public class TaskKanbanPageViewModel : CollectionViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly IPageDialogService _pageDialogService;
        private readonly ICurrentModelProvider<ITaskGroupModel> _currentTaskGroupProvider;
        private readonly IKanbanStateFacade _kanbanStateFacade;
        private readonly ITaskFacade _taskFacade;
        private readonly IToastMessageService _toastMessageService;
        private readonly DialogEventService _dialogEventService;

        public TaskKanbanPageViewModel(INavigationService navigationService, IDialogService dialogService, IPageDialogService pageDialogService,
            ICurrentModelProvider<ITaskGroupModel> currentTaskGroupProvider, IKanbanStateFacade kanbanStateFacade, ITaskFacade taskFacade, IToastMessageService toastMessageService,
            DialogEventService dialogEventService) : base(navigationService)
        {
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
            _currentTaskGroupProvider = currentTaskGroupProvider;
            _kanbanStateFacade = kanbanStateFacade;
            _taskFacade = taskFacade;
            _toastMessageService = toastMessageService;
            _dialogEventService = dialogEventService;
            BackCommand = new DelegateCommand<ITaskModel>(async (t) => await PushTaskBackAsync(t));
            CompleteCommand = new DelegateCommand<ITaskModel>(async (t) => await CompleteTaskAsync(t));
            DeleteTaskCommand = new DelegateCommand<ITaskModel>(async (t) => await DeleteTaskAsync(t));
            KanbanStateChangedCommand = new DelegateCommand<IKanbanStateModel>(async (t) => await KanbanStateChangedAsync(t));
            ClickChangeKanbanCommand = new DelegateCommand<IKanbanStateModel>(async (t) => await ClickChangeKanbanAsync(t));
            SelectTaskCommand = new DelegateCommand<ITaskModel>(OnSelectTask);
            RefreshCommand = new DelegateCommand(async () =>
            {
                BeginRefresh();
                await RefreshAsync(SelectedKanbanState);
                EndRefresh();
            });
            EditCommand = new DelegateCommand<ITaskModel>(async (s) => await EditAsync(s), (s) => s != null && !IsDialogThrown);
            InitDialogCommands();
        }


        public DelegateCommand RefreshCommand { get; set; }
        public DelegateCommand ClearTasksCommand { get; private set; }
        public DelegateCommand ShowAddTaskDialogCommand { get; private set; }
        public DelegateCommand<IKanbanStateModel> KanbanStateChangedCommand { get; }
        public DelegateCommand<ITaskModel> BackCommand { get; }
        public DelegateCommand<ITaskModel> CompleteCommand { get; }
        public DelegateCommand<ITaskModel> DeleteTaskCommand { get; }
        public DelegateCommand<ITaskModel> SelectTaskCommand { get; }
        public DelegateCommand<ITaskModel> EditCommand { get; }
        public DelegateCommand<IKanbanStateModel> ClickChangeKanbanCommand { get; }


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

        protected override async Task InitializeAsyncInt()
        {
            BeginProcess();
            await base.InitializeAsyncInt();
            if(KanbanStates.IsNullOrEmpty())
                KanbanStates = await _kanbanStateFacade.GetKanbanStatesByTaskGroupOrderedByStateAsync(_currentTaskGroupProvider.GetModel().Id).ToObservableCollectionAsync();
            EndProcess();
        }

        protected override async Task OnNavigatedToAsyncInt(INavigationParameters parameters)
        {
            await base.OnNavigatedFromAsyncInt(parameters);
            IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
            _dialogEventService.OnRaiseDialogEvent(dialogEvent, Tasks);
        }

        protected override void DestroyInt()
        {
            base.DestroyInt();
            DialogThrownEvent -= ShowAddTaskDialogCommand.RaiseCanExecuteChanged;
            DialogThrownEvent -= ClearTasksCommand.RaiseCanExecuteChanged;
        }

        private void InitDialogCommands()
        {
            ShowAddTaskDialogCommand = new DelegateCommand(async () => await ShowAddTaskDialogAsync(), () => !IsDialogThrown);
            DialogThrownEvent += ShowAddTaskDialogCommand.RaiseCanExecuteChanged;
            ClearTasksCommand = new DelegateCommand(async () => await ClearTasksAsync(), () => !IsDialogThrown);
            DialogThrownEvent += ClearTasksCommand.RaiseCanExecuteChanged;
        }

        private void UpdateButtonsVisibility(IKanbanStateModel kanbanState)
        {
            IsBackwardButtonVisible = kanbanState.StateOrder != 0;
            IsCompleteButtonVisible = _kanbanStateFacade.GetKanbanStatesByTaskGroup(_currentTaskGroupProvider.GetModel().Id).Max(s => s.StateOrder) != kanbanState.StateOrder;
            IsDeleteButtonVisible = _kanbanStateFacade.GetKanbanStatesByTaskGroup(_currentTaskGroupProvider.GetModel().Id).Max(s => s.StateOrder) == kanbanState.StateOrder;
        }

        private async Task PushTaskBackAsync(ITaskModel obj)
        {
            await MoveWithTask(obj, false);
        }

        private async Task CompleteTaskAsync(ITaskModel obj)
        {
            await MoveWithTask(obj, true);
        }

        private async Task MoveWithTask(ITaskModel obj, bool isMoveToNext)
        {
            IKanbanStateModel model = isMoveToNext ? await _kanbanStateFacade.GetNextKanbanStateAsync(_currentTaskGroupProvider.GetModel().Id, SelectedKanbanState.StateOrder) : await _kanbanStateFacade.GetPreviousKanbanStateAsync(_currentTaskGroupProvider.GetModel().Id, SelectedKanbanState.StateOrder);
            if (model != null)
            {
                Tasks.Remove(obj);
                obj.StateId = model.Id;
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
            IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddTaskDialogView", new KanbanStateNavigationParameters(SelectedKanbanState))).Parameters;
            IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
            _dialogEventService.OnRaiseDialogEvent(dialogEvent, Tasks);
            IsDialogThrown = false;
        }

        private async Task ClearTasksAsync()
        {
            BeginProcess();
            IsDialogThrown = true;
            if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning, TranslateViewModelsSR.TaskClearTasksMessage,
                TranslateViewModelsSR.DialogYes, TranslateViewModelsSR.DialogNo))
            {
                await _taskFacade.ClearTasksByKanbanStateAsync(SelectedKanbanState.Id);
                await RefreshAsync(SelectedKanbanState);
                _currentTaskGroupProvider.GetModel().TasksCount = 0;
            }
            IsDialogThrown = false;
            EndProcess();
        }

        private async Task DeleteTaskAsync(ITaskModel obj)
        {
            Tasks.Remove(obj);
            await _taskFacade.RemoveAsync(obj.Id);
            _currentTaskGroupProvider.GetModel().TasksCount--;
        }

        private async Task KanbanStateChangedAsync(IKanbanStateModel model)
        {
            if (model == null)
                return;
            BeginProcess();
            SelectedKanbanState = model;
            await RefreshAsync(model);
            UpdateButtonsVisibility(model);
            EndProcess();
        }

        private async Task ClickChangeKanbanAsync(IKanbanStateModel kanbanStateModel)
        {
            SelectedKanbanState = kanbanStateModel;
        }

        private void OnSelectTask(ITaskModel obj)
        {
            SelectedTask = obj;
        }

        private async Task EditAsync(ITaskModel task)
        {
            BeginProcess();
            await NavigationService.NavigateAsync("TaskDetailPage", new TaskNavigationParameters(new TaskModel(task)));
            EndProcess();
        }

        private async Task RefreshAsync(IKanbanStateModel model)
        {
            if (model == null)
                return;
            BeginProcess();
            Tasks = await _taskFacade.GetTasksByTaskGroupIdAndKanbanStateAsync(_currentTaskGroupProvider.GetModel().Id, model.Name).ToObservableCollectionAsync();
            EndProcess();
        }
    }
}