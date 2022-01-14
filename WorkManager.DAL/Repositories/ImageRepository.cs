using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

		protected override ICollection<ImageEntity> GetAllInt(IQueryable<ImageEntity> dbSet)
		{
			return dbSet.ToList();
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
			if (entity.Task?.State != null)
			{
				dbContext.Entry(entity.Task.State).State = EntityState.Unchanged;
			}
			if (entity.Task?.TaskGroup?.User != null)
			{
				dbContext.Entry(entity.Task.TaskGroup.User).State = EntityState.Unchanged;
			}
		}

		public ICollection<ImageEntity> GetAllImagesByTask(Guid id)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.ImageSet.Where(s => s.TaskId == id).ToList();
			}
		}

        public async Task<ICollection<ImageEntity>> GetAllImagesByTaskAsync(Guid id, CancellationToken token)
        {
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return await dbContext.ImageSet.Where(s => s.TaskId == id).ToListAsync(token);
            }
		}

        public uint GetImagesCountByTask(Guid taskId)
        {
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return (uint) dbContext.ImageSet.Count(s=>s.TaskId == taskId);
            }
		}
    }
}