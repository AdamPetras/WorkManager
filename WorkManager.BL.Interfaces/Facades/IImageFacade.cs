using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface IImageFacade : IFacade<IImageModel>
	{
        ICollection<IImageModel> GetAllImagesByTask(Guid id);
        Task<ICollection<IImageModel>> GetAllImagesByTaskAsync(Guid id, CancellationToken token = default);
	}
}