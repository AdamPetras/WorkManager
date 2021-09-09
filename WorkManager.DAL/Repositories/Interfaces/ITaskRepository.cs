using System;
using System.Collections.Generic;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface ITaskRepository : IRepository<TaskEntity>
	{
		IEnumerable<TaskEntity> GetTasksByTaskGroupId(Guid taskGroupId);
		IEnumerable<TaskEntity> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName);
	}
}