using System;
using System.Collections.Generic;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface IKanbanStateFacade : IFacade<IKanbanStateModel>
	{
		void CreateDefaultKanbanStateModels(ITaskGroupModel taskGroup);
		IEnumerable<IKanbanStateModel> GetKanbanStatesByTaskGroup(Guid taskGroupId);
	}
}