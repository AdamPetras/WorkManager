using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.NavigationParams;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Pages
{
    public class RelatedTasksPageViewModel : CollectionViewModelBase
    {
        private readonly ITaskFacade _taskFacade;
        private readonly ITaskDetailFacade _taskDetailFacade;

        private ITaskDetailModel _selectedTask;

        public RelatedTasksPageViewModel(INavigationService navigationService, ViewModelTaskExecute viewModelTaskExecute, ITaskFacade taskFacade, ITaskDetailFacade taskDetailFacade) : base(navigationService, viewModelTaskExecute)
        {
            _taskFacade = taskFacade;
            _taskDetailFacade = taskDetailFacade;
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
            SelectDeselectRelatedCommand = new DelegateCommand<ITaskModel>(async (s) => await SelectDeselectRelatedAsync(s));
        }

        private async Task SelectDeselectRelatedAsync(ITaskModel model)
        {
            BeginProcess();
            //ITaskModel foundedModel = _selectedTask.RelatedTasks.SingleOrDefault(s => s.Id == model.Id);
            //if (foundedModel == null)
            //{
            //    _selectedTask.RelatedTasks.Add(model);
            //    RelatedTasks.Remove(model);
            //}
            //else
            //{
            //    _selectedTask.RelatedTasks.Remove(foundedModel);
            //    RelatedTasks.Add(foundedModel);
            //}
            //await ViewModelTaskExecute.ExecuteTaskWithQueue(_selectedTask, _taskDetailFacade.UpdateAsync);
            EndProcess();
        }

        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand<ITaskModel> SelectDeselectRelatedCommand { get; }

        private ObservableCollection<ITaskModel> _relatedTasks;
        public ObservableCollection<ITaskModel> RelatedTasks
        {
            get => _relatedTasks;
            set
            {
                _relatedTasks = value;
                RaisePropertyChanged();
            }
        }

        protected override async Task OnNavigatedToAsyncInt(INavigationParameters parameters)
        {
            BeginProcess();
            await base.OnNavigatedToAsyncInt(parameters);
            TaskDetailNavigationParameters taskNavigationParameters = new TaskDetailNavigationParameters(parameters);
            _selectedTask = taskNavigationParameters.TaskDetailModel;
            await RefreshAsync();
            EndProcess();
        }

        private async Task RefreshAsync()
        {
            BeginProcess();
            BeginRefresh();
            RelatedTasks = new ObservableCollection<ITaskModel>(await ViewModelTaskExecute.ExecuteTaskWithQueue(_selectedTask, _selectedTask.TaskGroupId, _taskFacade.GetTasksByTaskGroupNoRelatedToTaskAsync));
            EndRefresh();
            EndProcess();
        }
    }
}