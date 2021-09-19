using System;
using System.Collections.Generic;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IImageRepository : IRepository<ImageEntity>
	{
		IEnumerable<ImageEntity> GetAllImagesByTask(Guid id);
	}
}