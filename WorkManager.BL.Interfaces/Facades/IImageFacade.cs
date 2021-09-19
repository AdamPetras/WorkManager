using System;
using System.Collections.Generic;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface IImageFacade : IFacade<IImageModel>
	{
		ICollection<IImageModel> GetAllImagesByTask(Guid id);
	}
}