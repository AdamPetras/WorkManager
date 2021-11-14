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
using WorkManager.BL.NavigationParams;
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
			DeleteTaskGroupCommand = new DelegateCommand<ITaskGroupModel>(async (t) => await DeleteTaskGroupAsync(t));
			EditCommand = new DelegateCommand<ITaskGroupModel>(async(s) => await EditAsync(s));
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
			InitDialogCommands();
		}

		public DelegateCommand ShowAddTaskGroupDialogCommand { get; private set; }
		public DelegateCommand ClearTaskGroupsCommand { get; private set; }
		public DelegateCommand RefreshCommand { get; }
        public DelegateCommand<ITaskGroupModel> DeleteTaskGroupCommand { get; }
		public DelegateCommand<ITaskGroupModel> EditCommand { get; }
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

        protected override async Task InitializeAsyncInt()
		{
			await base.InitializeAsyncInt( );
                await RefreshAsync();
		}

		protected override async void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
            if (TaskGroups == null || TaskGroups.Count == 0)
                await RefreshAsync();
        }

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= ShowAddTaskGroupDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ClearTaskGroupsCommand.RaiseCanExecuteChanged;
		}

		private void InitDialogCommands()
		{
			ShowAddTaskGroupDialogCommand = new DelegateCommand(async () => await ShowAddTaskGroupDialog(), () => !IsDialogThrown);
			DialogThrownEvent += ShowAddTaskGroupDialogCommand.RaiseCanExecuteChanged;
			ClearTaskGroupsCommand = new DelegateCommand(async () => await ClearTaskGroupsAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ClearTaskGroupsCommand.RaiseCanExecuteChanged;
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

		private async Task ClearTaskGroupsAsync()
		{
			BeginProcess();
			IsDialogThrown = true;
			if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
				TranslateViewModelsSR.TaskGroupClearDialogMessage, TranslateViewModelsSR.DialogYes,
				TranslateViewModelsSR.DialogNo))
			{
				await _taskGroupFacade.ClearAsync();
				TaskGroups.Clear();
			}
			IsDialogThrown = false;
			EndProcess();
		}

		private async Task DeleteTaskGroupAsync(ITaskGroupModel taskGroupModel)
		{
			if (taskGroupModel != null)
			{
			    await _taskGroupFacade.RemoveAsync(taskGroupModel.Id);
			    TaskGroups.Remove(taskGroupModel);
			}
		}

        private async Task RefreshAsync()
		{
			BeginProcess();
			TaskGroups = new ObservableCollection<ITaskGroupModel>(await _taskGroupFacade.GetTaskGroupsByUserIdAsync(_currentUserProvider.GetModel().Id));
			EndProcess();
		}

		private async Task EditAsync(ITaskGroupModel taskGroupModel)
		{
            BeginProcess();
            await NavigationService.NavigateAsync("TaskGroupDetailPage", new TaskGroupNavigationParameters(taskGroupModel));
            EndProcess();
		}
	}
}