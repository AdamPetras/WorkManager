using Prism.Common;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.NavigationParams
{
    public class TaskDetailNavigationParameters : ParametersBase, IDialogParameters, INavigationParameters
    {
        public ITaskDetailModel TaskDetailModel { get; }

        public TaskDetailNavigationParameters(ITaskDetailModel taskDetailModel)
        {
            TaskDetailModel = taskDetailModel;
            Add(nameof(TaskDetailModel), taskDetailModel);
        }

        public TaskDetailNavigationParameters(IParameters parameters)
        {
            TaskDetailModel = parameters.GetValue<ITaskDetailModel>(nameof(TaskDetailModel));
        }
    }
}