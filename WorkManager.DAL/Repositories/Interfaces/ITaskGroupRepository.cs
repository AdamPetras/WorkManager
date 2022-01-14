using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface ITaskGroupRepository : IRepository<TaskGroupEntity>
	{
        ICollection<TaskGroupEntity> GetTaskGroupsByUserId(Guid userId);
        Task<ICollection<TaskGroupEntity>> GetTaskGroupsByUserIdAsync(Guid userId, CancellationToken token);
        Task<bool> ExistsAsync(string taskGroupName, CancellationToken token = default);
    }
}