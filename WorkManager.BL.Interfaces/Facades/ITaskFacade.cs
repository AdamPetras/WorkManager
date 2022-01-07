using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface ITaskFacade: IFacade<ITaskModel>
	{
        IEnumerable<ITaskModel> GetTasksByTaskGroupId(Guid taskGroupId);
        IEnumerable<ITaskModel> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName);
		Task<IEnumerable<ITaskModel>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken cancellationToken = default);
        Task ClearTasksByKanbanStateAsync(Guid kanbanStateId, CancellationToken token = default);
    }
}