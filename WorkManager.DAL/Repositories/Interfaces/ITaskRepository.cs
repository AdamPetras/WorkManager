using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface ITaskRepository : IRepository<TaskEntity>
	{
		IEnumerable<TaskEntity> GetTasksByTaskGroupId(Guid taskGroupId);
		IEnumerable<TaskEntity> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName);
		Task<IEnumerable<TaskEntity>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken cancellationToken = default);
	}
}