using Prism.Common;
using Prism.Navigation;
using Prism.Navigation.Xaml;
using Prism.Services.Dialogs;
using WorkManager.Models.Interfaces;
using NavigationParameters = Prism.Navigation.NavigationParameters;

namespace WorkManager.BL.NavigationParams
{
    public class TaskNavigationParameters : ParametersBase, IDialogParameters, INavigationParameters
    {
        public ITaskModel TaskModel { get; }

        public TaskNavigationParameters(ITaskModel taskModel)
        {
            TaskModel = taskModel;
            Add("Task", taskModel);
        }

        public TaskNavigationParameters(IParameters parameters)
        {
            TaskModel = parameters.GetValue<ITaskModel>("Task");
        }
    }
}