using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface ITaskGroupFacade : IFacade<ITaskGroupModel>
	{
        IEnumerable<ITaskGroupModel> GetTaskGroupsByUserId(Guid userId);
        IAsyncEnumerable<ITaskGroupModel> GetTaskGroupsByUserIdAsync(Guid id, CancellationToken token = default);
        Task<bool> ExistsAsync(string taskGroupName, CancellationToken token = default);
    }
}