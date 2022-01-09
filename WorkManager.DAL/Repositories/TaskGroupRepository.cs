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
	public class TaskGroupRepository : RepositoryBase<TaskGroupEntity>, ITaskGroupRepository
	{
		public TaskGroupRepository(IDBContextFactory<WorkManagerDbContext> idbContextFactory) : base(idbContextFactory)
		{
		}

		public ICollection<TaskGroupEntity> GetTaskGroupsByUserId(Guid userId)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.TaskGroupSet.AsQueryable().Where(s => s.User.Id == userId).Include(s => s.User).AsNoTracking().ToList();
			}
		}

        public async Task<ICollection<TaskGroupEntity>> GetTaskGroupsByUserIdAsync(Guid userId, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
            {
                return await dbContext.TaskGroupSet.AsQueryable().Where(s => s.User.Id == userId).Include(s => s.User).AsNoTracking().ToListAsync(token);
            }
		}

        protected override ICollection<TaskGroupEntity> GetAllInt(IQueryable<TaskGroupEntity> dbSet)
		{
			return dbSet.Include(s => s.User).ToList();
		}

		protected override void AddInt(TaskGroupEntity entity, WorkManagerDbContext dbContext)
		{
			if(entity.User != null)
			{
				dbContext.UserSet.Attach(entity.User);
			}
		}
	}
}