using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
    public interface ITaskDetailFacade : IFacade<ITaskDetailModel>
    {
        Task<ITaskDetailModel> GetByIdAsync(Guid taskId, CancellationToken token);
    }
}