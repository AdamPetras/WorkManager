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
		private readonly DialogEventService _dialogEventService;

		public TaskGroupPageViewModel(INavigationService navigationService, ICurrentModelProvider<IUserModel> currentUserProvider, IPageDialogService pageDialogService
			, ICurrentModelProviderManager<ITaskGroupModel> currentTaskGroupProvider, IDialogService dialogService, ITaskGroupFacade taskGroupFacade, DialogEventService dialogEventService) : base(navigationService)
		{
			_currentUserProvider = currentUserProvider;
			_pageDialogService = pageDialogService;
			_currentTaskGroupProvider = currentTaskGroupProvider;
			_dialogService = dialogService;
			_taskGroupFacade = taskGroupFacade;
			_dialogEventService = dialogEventService;
			NavigateToTasksPageCommand = new DelegateCommand<ITaskGroupModel>(async (model)=> await NavigateToTasksPageAsync(model));
			SelectTaskGroupCommand = new DelegateCommand<ITaskGroupModel>(SelectTaskGroup);
			EditCommand = new DelegateCommand(Edit, () => SelectedTaskGroup != null);
			InitDialogCommands();
		}

		public DelegateCommand<ITaskGroupModel> SelectTaskGroupCommand { get; }
		public DelegateCommand ShowAddTaskGroupDialogCommand { get; private set; }
		public DelegateCommand ClearWholeOrDeleteSingleTaskGroupCommand { get; private set; }
		public DelegateCommand EditCommand { get; }
		public DelegateCommand<ITaskGroupModel> NavigateToTasksPageCommand { get; }

		private ObservableCollection<ITaskGroupModel> _taskGroups;

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

		private ITaskGroupModel _selectedTaskGroup;
		public ITaskGroupModel SelectedTaskGroup
		{
			get => _selectedTaskGroup;
			set
			{
				if (_selectedTaskGroup == value) return;
				_selectedTaskGroup = value;
				RaisePropertyChanged();
				EditCommand.RaiseCanExecuteChanged();
			}
		}

		protected override void InitializeInt()
		{
			base.InitializeInt();
			TaskGroups = new ObservableCollection<ITaskGroupModel>(_taskGroupFacade.GetTaskGroupsByUserId(_currentUserProvider.GetModel().Id));
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			if (TaskGroups == null || TaskGroups.Count == 0)
				TaskGroups = new ObservableCollection<ITaskGroupModel>(_taskGroupFacade.GetTaskGroupsByUserId(_currentUserProvider.GetModel().Id));
		}

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= ShowAddTaskGroupDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ClearWholeOrDeleteSingleTaskGroupCommand.RaiseCanExecuteChanged;
		}

		private void InitDialogCommands()
		{
			ShowAddTaskGroupDialogCommand = new DelegateCommand(async () => await ShowAddTaskGroupDialog(), () => !IsDialogThrown);
			DialogThrownEvent += ShowAddTaskGroupDialogCommand.RaiseCanExecuteChanged;
			ClearWholeOrDeleteSingleTaskGroupCommand = new DelegateCommand(async () => await ClearWholeOrDeleteSingleTaskGroupAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ClearWholeOrDeleteSingleTaskGroupCommand.RaiseCanExecuteChanged;
		}

		private async Task NavigateToTasksPageAsync(ITaskGroupModel obj)
		{
			BeginProcess();
			_currentTaskGroupProvider.SetItem(obj);
			await NavigationService.NavigateAsync("TaskKanbanPage");
			EndProcess();
		}

		private async Task ShowAddTaskGroupDialog()
		{
			IsDialogThrown = true;
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddTaskGroupDialogView")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent,TaskGroups);
			IsDialogThrown = false;
		}

		private void SelectTaskGroup(ITaskGroupModel model)
		{
			SelectedTaskGroup = model;
		}

		private async Task ClearWholeOrDeleteSingleTaskGroupAsync()
		{
			BeginProcess();
			if (_selectedTaskGroup != null)
			{
				if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
					TranslateViewModelsSR.SelectedTaskGroupDeleteDialogMessageFormat(_selectedTaskGroup.Name), TranslateViewModelsSR.DialogYes,
					TranslateViewModelsSR.DialogNo))
				{
					await _taskGroupFacade.RemoveAsync(_selectedTaskGroup.Id);
					TaskGroups.Remove(_selectedTaskGroup);
					_selectedTaskGroup = null;
				}
			}
			else
			{
				IsDialogThrown = true;
				if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
					TranslateViewModelsSR.TaskGroupClearDialogMessage, TranslateViewModelsSR.DialogYes,
					TranslateViewModelsSR.DialogNo))
				{
					await _taskGroupFacade.ClearAsync();
					TaskGroups.Clear();
				}
				IsDialogThrown = false;
			}
			EndProcess();
		}

		private void Edit()
		{

		}
	}
}