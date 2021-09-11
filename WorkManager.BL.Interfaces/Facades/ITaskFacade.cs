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
		ICollection<ITaskModel> GetTasksByTaskGroupId(Guid taskGroupId);
		ICollection<ITaskModel> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName);
		Task<ICollection<ITaskModel>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken cancellationToken = default);
	}
}