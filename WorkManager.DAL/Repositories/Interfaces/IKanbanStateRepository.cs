using System;
using System.Collections.Generic;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IKanbanStateRepository: IRepository<KanbanStateEntity>
	{
		IEnumerable<KanbanStateEntity> GetKanbanStateByTaskGroup(Guid taskGroupId);
	}
}