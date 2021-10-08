using System;
using System.Collections.Generic;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface IKanbanStateFacade : IFacade<IKanbanStateModel>
	{
		IEnumerable<IKanbanStateModel> GetDefaultKanbanStateModels();
		IEnumerable<IKanbanStateModel> GetKanbanStateByTaskGroup(Guid taskGroupId);
	}
}