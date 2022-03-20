using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.NavigationParams;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Pages
{
    public class RelatedTasksPageViewModel : CollectionViewModelBase
    {
        private readonly ITaskFacade _taskFacade;
        private readonly IRelatedTaskFacade _relatedTaskFacade;

        private ITaskModel _selectedTask;

        public RelatedTasksPageViewModel(INavigationService navigationService, ViewModelTaskExecute viewModelTaskExecute, ITaskFacade taskFacade, IRelatedTaskFacade relatedTaskFacade) : base(navigationService, viewModelTaskExecute)
        {
            _taskFacade = taskFacade;
            _relatedTaskFacade = relatedTaskFacade;
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
            SelectDeselectRelatedCommand = new DelegateCommand<ITaskModel>(async (s) => await SelectDeselectRelatedAsync(s));
        }

        private async Task SelectDeselectRelatedAsync(ITaskModel model)
        {
            BeginProcess();
            IRelatedTaskModel foundedModel = await ViewModelTaskExecute.ExecuteTaskWithQueue(_selectedTask.RelatedTaskId, _relatedTaskFacade.GetByIdAsync);
            if (foundedModel.RelatedBy.Any(s => s.Id == model.Id))
            {
                foundedModel.RelatedBy.Remove(model);
            }
            else
            {
                foundedModel.RelatedBy.Add(model);
            }
            await ViewModelTaskExecute.ExecuteTaskWithQueue(foundedModel, _relatedTaskFacade.UpdateAsync);
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
            TaskNavigationParameters taskNavigationParameters = new TaskNavigationParameters(parameters);
            _selectedTask = taskNavigationParameters.TaskModel;
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