using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IKanbanTaskGroupRepository : IRepository<KanbanTaskGroupEntity>
	{
		IEnumerable<KanbanTaskGroupEntity> GetKanbansByTaskGroupId(Guid taskGroupId);
		Task<IEnumerable<KanbanTaskGroupEntity>> GetKanbansByTaskGroupIdAsync(Guid taskGroupId, CancellationToken token = default);
	}
}