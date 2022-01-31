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
				return dbContext.TaskGroupSet.AsQueryable().Where(s => s.UserId == userId).ToList();
			}
		}

        public IAsyncEnumerable<TaskGroupEntity> GetTaskGroupsByUserIdAsync(Guid userId, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return dbContext.TaskGroupSet.AsQueryable().Where(s => s.UserId == userId).ToList().ToAsyncEnumerable();
			}
		}

        public async Task<bool> ExistsAsync(string taskGroupName, CancellationToken token = default)
        {
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return await dbContext.TaskGroupSet.AsQueryable().AnyAsync(s=>s.Name == taskGroupName, token);
            }
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