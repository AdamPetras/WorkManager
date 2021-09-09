using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface IKanbanTaskGroupFacade : IFacade<IKanbanTaskGroupModel>
	{
		ICollection<IKanbanTaskGroupModel> GetKanbansByTaskGroupId(Guid taskGroupId);
		Task<ICollection<IKanbanTaskGroupModel>> GetKanbansByTaskGroupIdAsync(Guid taskGroupId, CancellationToken token = default);
	}
}