using Prism.Common;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.NavigationParams
{
    public class KanbanStateNavigationParameters : ParametersBase, IDialogParameters, INavigationParameters
    {
        public IKanbanStateModel KanbanState { get; }

        public KanbanStateNavigationParameters(IKanbanStateModel kanbanState)
        {
            KanbanState = kanbanState;
            Add(nameof(KanbanState), kanbanState);
        }

        public KanbanStateNavigationParameters(IParameters parameters)
        {
            KanbanState = parameters.GetValue<IKanbanStateModel>(nameof(KanbanState));
        }
    }
}
