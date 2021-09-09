using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.BaseClasses;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories
{
	public class TaskGroupRepository : RepositoryBase<TaskGroupEntity>, ITaskGroupRepository
	{
		public TaskGroupRepository(DbContext.Interfaces.IDbContextFactory<WorkManagerDbContext> dbContextFactory) : base(dbContextFactory)
		{
		}

		public IEnumerable<TaskGroupEntity> GetTaskGroupsByUserId(Guid userId)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return dbContext.TaskGroupSet.Where(s => s.User.Id == userId).Include(s => s.User).AsNoTracking().ToList();
			}
		}

		protected override IEnumerable<TaskGroupEntity> GetAllInt(IQueryable<TaskGroupEntity> dbSet)
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