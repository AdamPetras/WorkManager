using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Services;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class TaskGroupPageViewModel : ViewModelBase
	{
		private readonly ICurrentModelProvider<IUserModel> _currentUserProvider;
		private readonly IPageDialogService _pageDialogService;
		private readonly ICurrentModelProviderManager<ITaskGroupModel> _currentTaskGroupProvider;
		private readonly IDialogService _dialogService;
		private readonly ITaskGroupFacade _taskGroupFacade;
		private readonly IEventAggregator _eventAggregator;
		private readonly DialogEventService _dialogEventService;

		public TaskGroupPageViewModel(INavigationService navigationService, ICurrentModelProvider<IUserModel> currentUserProvider, IPageDialogService pageDialogService
			, ICurrentModelProviderManager<ITaskGroupModel> currentTaskGroupProvider, IDialogService dialogService, ITaskGroupFacade taskGroupFacade,
		IEventAggregator eventAggregator, DialogEventService dialogEventService) : base(navigationService)
		{
			_currentUserProvider = currentUserProvider;
			_pageDialogService = pageDialogService;
			_currentTaskGroupProvider = currentTaskGroupProvider;
			_dialogService = dialogService;
			_taskGroupFacade = taskGroupFacade;
			_eventAggregator = eventAggregator;
			_dialogEventService = dialogEventService;
			ShowAddTaskGroupDialogCommand = new DelegateCommand(async()=>await ShowAddTaskGroupDialog());
			ClearWholeTaskGroupsCommand = new DelegateCommand(async () => await ClearWholeTaskGroupsAsync());
			NavigateToTasksPageCommand = new DelegateCommand<ITaskGroupModel>(async (model)=> await NavigateToTasksPageAsync(model));
			SelectedTaskGroupCommand = new DelegateCommand<ITaskGroupModel>(async (t) => await SelectTaskAsync(t));
		}

		public DelegateCommand<ITaskGroupModel> SelectedTaskGroupCommand { get; }
		public DelegateCommand ShowAddTaskGroupDialogCommand { get; }
		public DelegateCommand ClearWholeTaskGroupsCommand { get; }

		public DelegateCommand<ITaskGroupModel> NavigateToTasksPageCommand { get; }

		private ObservableCollection<ITaskGroupModel> _taskGroups;
		private ITaskGroupModel _selectedTaskGroup;

		public ObservableCollection<ITaskGroupModel> TaskGroups
		{
			get => _taskGroups;
			private set
			{
				if (_taskGroups == value) return;
				_taskGroups = value;
				RaisePropertyChanged();
			}
		}



		protected override void InitializeInt()
		{
			base.InitializeInt();
			TaskGroups = new ObservableCollection<ITaskGroupModel>(_taskGroupFacade.GetTaskGroupsByUserId(_currentUserProvider.GetModel().Id));
		}

		private async Task NavigateToTasksPageAsync(ITaskGroupModel obj)
		{
			_currentTaskGroupProvider.SetItem(obj);
			await NavigationService.NavigateAsync("TaskKanbanPage");
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			if (TaskGroups.Count == 0)
				TaskGroups = new ObservableCollection<ITaskGroupModel>(_taskGroupFacade.GetTaskGroupsByUserId(_currentUserProvider.GetModel().Id));
		}

		private async Task ShowAddTaskGroupDialog()
		{
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddTaskGroupDialogView")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent,TaskGroups);
		}

		private async Task SelectTaskAsync(ITaskGroupModel model)
		{
			_selectedTaskGroup = model;
		}

		private async Task ClearWholeTaskGroupsAsync()
		{
			if (_selectedTaskGroup != null)
			{
				await _taskGroupFacade.RemoveAsync(_selectedTaskGroup.Id);
				TaskGroups.Remove(_selectedTaskGroup);
			}
			else
			{
				if (await _pageDialogService.DisplayAlertAsync(TaskGroupPageViewModelSR.ClearDialogTitle,
					TaskGroupPageViewModelSR.ClearDialogMessage, TaskGroupPageViewModelSR.DialogYes,
					TaskGroupPageViewModelSR.DialogNo))
				{
					_taskGroupFacade.Clear();
					TaskGroups.Clear();
				}
			}
		}
	}
}