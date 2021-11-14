using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.NavigationParams;
using WorkManager.BL.Services;
using WorkManager.Core;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;
using Xamarin.Forms.Internals;

namespace WorkManager.ViewModels.Pages
{
    public class TaskGroupDetailPageViewModel : ViewModelBase
    {
        private readonly ITaskGroupFacade _taskGroupFacade;
        private readonly IKanbanStateFacade _kanbanStateFacade;
        private readonly IDialogService _dialogService;
        private readonly DialogEventService _dialogEventService;
        private readonly IToastMessageService _toastMessageService;

        public TaskGroupDetailPageViewModel(INavigationService navigationService, ITaskGroupFacade taskGroupFacade, IKanbanStateFacade kanbanStateFacade, 
            IDialogService dialogService, DialogEventService dialogEventService, IToastMessageService toastMessageService) : base(navigationService)
        {
            _taskGroupFacade = taskGroupFacade;
            _kanbanStateFacade = kanbanStateFacade;
            _dialogService = dialogService;
            _dialogEventService = dialogEventService;
            _toastMessageService = toastMessageService;
            SaveCommand = new DelegateCommand(async () => await SaveAsync());
            DeleteCommand = new DelegateCommand(async () => await DeleteAsync());
            AddKanbanCommand = new DelegateCommand(async () => await AddKanbanAsync());
            DeleteKanbanStateCommand = new DelegateCommand<IKanbanStateModel>(async (s) => await DeleteKanbanStateAsync(s));
            MoveDownKanbanStateCommand = new DelegateCommand<IKanbanStateModel>(MoveDownKanbanState);
            MoveUpKanbanStateCommand = new DelegateCommand<IKanbanStateModel>(MoveUpKanbanState);
        }

        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand DeleteCommand { get; }
        public DelegateCommand AddKanbanCommand { get; }
        public DelegateCommand<IKanbanStateModel> DeleteKanbanStateCommand { get; }
        public DelegateCommand<IKanbanStateModel> MoveUpKanbanStateCommand { get; }
        public DelegateCommand<IKanbanStateModel> MoveDownKanbanStateCommand { get; }


        private ITaskGroupModel _selectedTaskGroup;
        public ITaskGroupModel SelectedTaskGroup
        {
            get => _selectedTaskGroup;
            set
            {
                if (_selectedTaskGroup == value) return;
                _selectedTaskGroup = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<IKanbanStateModel> _kanbanItems;
        public ObservableCollection<IKanbanStateModel> KanbanItems
        {
            get => _kanbanItems;
            set
            {
                if (_kanbanItems == value) return;
                _kanbanItems = value;
                RaisePropertyChanged();
            }
        }

        protected override async void OnNavigatedToInt(INavigationParameters parameters)
        {
            base.OnNavigatedToInt(parameters);
            TaskGroupNavigationParameters navParams = new TaskGroupNavigationParameters(parameters);
            if (navParams.TaskGroupModel != null)
            {
                SelectedTaskGroup = navParams.TaskGroupModel;
                KanbanItems = new ObservableCollection<IKanbanStateModel>((await _kanbanStateFacade.GetKanbanStatesByTaskGroupAsync(SelectedTaskGroup.Id)).OrderBy(s => s.StateOrder));
            }
        }

        private async Task DeleteAsync()
        {
            BeginProcess();
            await _taskGroupFacade.RemoveAsync(SelectedTaskGroup.Id);
            KanbanItems.ForEach(async (s)=>await _kanbanStateFacade.RemoveAsync(s.Id));
            await NavigationService.GoBackAsync();
            EndProcess();
        }

        private async Task AddKanbanAsync()
        {
            BeginProcess();
            IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddKanbanStateDialogView",  new StateOrderTaskGroupNavigationParameters(KanbanItems.Count,SelectedTaskGroup))).Parameters;
            IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
            if (dialogEvent != null)
            {
                _dialogEventService.OnRaiseDialogEvent(dialogEvent, KanbanItems);
            }
            EndProcess();
        }

        private async Task SaveAsync()
        {
            BeginProcess();
            await _taskGroupFacade.UpdateAsync(SelectedTaskGroup);
            await TaskSaveKanbanStatesAsync();
            await NavigationService.GoBackAsync();
            EndProcess();
        }

        private async Task TaskSaveKanbanStatesAsync()
        {
            EnumerableDiffChecker<IKanbanStateModel> kanbanDiffChecker = new EnumerableDiffChecker<IKanbanStateModel>();
            DifferentialCollection<IKanbanStateModel> value = kanbanDiffChecker.CheckCollectionDifference(await _kanbanStateFacade.GetKanbanStatesByTaskGroupAsync(_selectedTaskGroup.Id), KanbanItems, (s, t) => s.Id == t.Id && s.StateOrder == t.StateOrder);
            foreach (IKanbanStateModel kanbanStateModel in value.DeleteEnumerable)
            {
                await _kanbanStateFacade.RemoveAsync(kanbanStateModel.Id);
            }
            foreach (IKanbanStateModel kanbanStateModel in value.AddEnumerable)
            {
                await _kanbanStateFacade.AddAsync(kanbanStateModel);
            }
        }

        private async Task DeleteKanbanStateAsync(IKanbanStateModel kanbanStateModel)
        {
            BeginProcess();
            if (KanbanItems.Count <= 2)
            {
                _toastMessageService.LongAlert(TranslateViewModelsSR.KanbanMustHaveThreeStates);
                return;
            }
            await _kanbanStateFacade.RemoveAsync(kanbanStateModel.Id);
            KanbanItems.Remove(kanbanStateModel);
            UpdateKanbanStatesOrder();
            EndProcess();
        }

        private void MoveUpKanbanState(IKanbanStateModel kanbanStateModel)
        {
            BeginProcess();
            int index = KanbanItems.IndexOf(kanbanStateModel);
            if (index == 0)
            {
                _toastMessageService.LongAlert(TranslateViewModelsSR.SwipeDirectionIsOutOfCollection);
                return;
            }
            KanbanItems.Remove(kanbanStateModel);
            KanbanItems.Insert(index - 1, kanbanStateModel);
            UpdateKanbanStatesOrder();
            EndProcess();
        }

        private void MoveDownKanbanState(IKanbanStateModel kanbanStateModel)
        {
            BeginProcess();
            int index = KanbanItems.IndexOf(kanbanStateModel);
            if (index == KanbanItems.Count - 1)
            {
                _toastMessageService.LongAlert(TranslateViewModelsSR.SwipeDirectionIsOutOfCollection);
                return;
            }
            KanbanItems.Remove(kanbanStateModel);
            KanbanItems.Insert(index + 1, kanbanStateModel);
            UpdateKanbanStatesOrder();
            EndProcess();
        }

        private void UpdateKanbanStatesOrder()
        {
            KanbanItems.ForEach(s=> s.StateOrder = KanbanItems.IndexOf(s));
        }
    }
}