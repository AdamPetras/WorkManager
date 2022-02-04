using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface ITaskGroupRepository : IRepository<TaskGroupEntity>
	{
    }
}