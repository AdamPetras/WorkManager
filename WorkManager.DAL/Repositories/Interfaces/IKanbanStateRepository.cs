using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IKanbanStateRepository: IRepository<KanbanStateEntity>
	{
        ICollection<KanbanStateEntity> GetKanbanStateByTaskOrderedByStateGroup(Guid taskGroupId);
        IAsyncEnumerable<KanbanStateEntity> GetKanbanStateByTaskGroupOrderedByStateAsync(Guid taskGroupId, CancellationToken token);
        KanbanStateEntity GetNextKanbanState(Guid taskGroupId, int currentStateOrder);
        Task<KanbanStateEntity> GetNextKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token);
        KanbanStateEntity GetPreviousKanbanState(Guid taskGroupId, int currentStateOrder);
        Task<KanbanStateEntity> GetPreviousKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token);
    }
}