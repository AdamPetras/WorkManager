using Prism.Common;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.NavigationParams
{
    public class StateOrderTaskGroupNavigationParameters : ParametersBase, IDialogParameters, INavigationParameters
    {
        public int StateOrder { get; }
        public ITaskGroupModel TaskGroup { get; }

        public StateOrderTaskGroupNavigationParameters(int stateOrder, ITaskGroupModel taskGroup)
        {
            StateOrder = stateOrder;
            TaskGroup = taskGroup;
            Add(nameof(StateOrder),stateOrder);
            Add(nameof(TaskGroup), taskGroup);
        }

        public StateOrderTaskGroupNavigationParameters(IParameters parameters)
        {
            StateOrder = parameters.GetValue<int>(nameof(StateOrder));
            TaskGroup = parameters.GetValue<ITaskGroupModel>(nameof(TaskGroup));
        }
    }
}