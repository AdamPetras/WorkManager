using Prism.Common;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.NavigationParams
{
    public class TaskGroupNavigationParameters : ParametersBase, IDialogParameters, INavigationParameters
    {
        public ITaskGroupModel TaskGroupModel { get; }

        public TaskGroupNavigationParameters(ITaskGroupModel taskGroupModel)
        {
            TaskGroupModel = taskGroupModel;
            Add(nameof(TaskGroupModel), taskGroupModel);
        }

        public TaskGroupNavigationParameters(IParameters parameters)
        {
            TaskGroupModel = parameters.GetValue<ITaskGroupModel>(nameof(TaskGroupModel));
        }
    }
}