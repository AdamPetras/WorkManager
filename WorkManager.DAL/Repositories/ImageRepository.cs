using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.DbContext.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.BaseClasses;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories
{
	public class ImageRepository : RepositoryBase<ImageEntity>, IImageRepository
	{
		public ImageRepository(IDBContextFactory<WorkManagerDbContext> idbContextFactory) : base(idbContextFactory)
		{
		}

		protected override IEnumerable<ImageEntity> GetAllInt(IQueryable<ImageEntity> dbSet)
		{
			return dbSet.Include(s => s.Task).ToList();
		}

		protected override void AddInt(ImageEntity entity, WorkManagerDbContext dbContext)
		{
			if (entity.Task != null)
			{
				dbContext.Entry(entity.Task).State = EntityState.Unchanged;
			}
			if (entity.Task?.TaskGroup != null)
			{
				dbContext.Entry(entity.Task.TaskGroup).State = EntityState.Unchanged;
			}
			if (entity.Task?.TaskGroup?.User != null)
			{
				dbContext.Entry(entity.Task.TaskGroup.User).State = EntityState.Unchanged;
			}
		}
	}
}