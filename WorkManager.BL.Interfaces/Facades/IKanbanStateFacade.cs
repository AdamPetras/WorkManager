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
		IEnumerable<IKanbanStateModel> GetKanbanStatesByTaskGroup(Guid taskGroupId);
        Task<IEnumerable<IKanbanStateModel>> GetKanbanStatesByTaskGroupAsync(Guid taskGroupId, CancellationToken token = default);
    }
}