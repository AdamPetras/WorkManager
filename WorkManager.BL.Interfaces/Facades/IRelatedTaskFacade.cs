using System;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
    public interface IRelatedTaskFacade : IFacade<IRelatedTaskModel>
    {
        Task<IRelatedTaskModel> GetByIdAsync(Guid relatedTaskId, CancellationToken token);
    }
}