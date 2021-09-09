using System;
using System.Collections.Generic;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface ITaskGroupFacade : IFacade<ITaskGroupModel>
	{
		ICollection<ITaskGroupModel> GetTaskGroupsByUserId(Guid userId);
	}
}