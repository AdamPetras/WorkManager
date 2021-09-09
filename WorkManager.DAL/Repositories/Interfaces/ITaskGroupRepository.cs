using System;
using System.Collections.Generic;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface ITaskGroupRepository : IRepository<TaskGroupEntity>
	{
		IEnumerable<TaskGroupEntity> GetTaskGroupsByUserId(Guid userId);
	}
}