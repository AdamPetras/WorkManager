using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
    public interface IKanbanStateFacade : IFacade<IKanbanStateModel>
    {
        void CreateDefaultKanbanStateModels(ITaskGroupModel taskGroup);
        ICollection<IKanbanStateModel> GetKanbanStatesByTaskGroup(Guid taskGroupId);
        Task<ICollection<IKanbanStateModel>> GetKanbanStatesByTaskGroupOrderedByStateAsync(Guid taskGroupId, CancellationToken token = default);
        IKanbanStateModel GetNextKanbanState(Guid taskGroupId, int currentStateOrder);
        Task<IKanbanStateModel> GetNextKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token = default);
        IKanbanStateModel GetPreviousKanbanState(Guid taskGroupId, int currentStateOrder);
        Task<IKanbanStateModel> GetPreviousKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token = default);
    }
}