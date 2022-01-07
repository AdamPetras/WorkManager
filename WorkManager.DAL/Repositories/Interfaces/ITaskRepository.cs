using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface ITaskRepository : IRepository<TaskEntity>
	{
        ICollection<TaskEntity> GetTasksByTaskGroupId(Guid taskGroupId);
        ICollection<TaskEntity> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName);
		Task<ICollection<TaskEntity>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken cancellationToken = default);
        uint GetTasksCountByTaskGroupId(Guid taskGroupId);
        Task ClearTasksByKanbanStateAsync(Guid kanbanStateId, CancellationToken token);
    }
}