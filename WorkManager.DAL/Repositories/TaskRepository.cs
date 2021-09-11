using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.BaseClasses;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories
{
	public class TaskRepository : RepositoryBase<TaskEntity>, ITaskRepository
	{
		public TaskRepository(DbContext.Interfaces.IDbContextFactory<WorkManagerDbContext> dbContextFactory) : base(dbContextFactory)
		{
		}

		protected override IEnumerable<TaskEntity> GetAllInt(IQueryable<TaskEntity> dbSet)
		{
			return dbSet.Include(s => s.TaskGroup).ToList();
		}

		public override TaskEntity Add(TaskEntity entity)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				if (entity == null)
					throw new ArgumentNullException();
				if (!Exists(entity))
				{
					dbContext.Entry(entity.State).State = EntityState.Unchanged;
					if (dbContext.TaskSet.Add(entity) != null)
					{
						AddInt(entity, dbContext);
						dbContext.SaveChanges();
						return entity;
					}
				}
			}
			return default;
		}

		protected override void AddInt(TaskEntity entity, WorkManagerDbContext dbContext)
		{
			if (entity.TaskGroup != null)
			{
				dbContext.Entry(entity.TaskGroup.User).State = EntityState.Unchanged;
				dbContext.Entry(entity.TaskGroup).State = EntityState.Unchanged;
			}
		}

		public IEnumerable<TaskEntity> GetTasksByTaskGroupId(Guid taskGroupId)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return dbContext.TaskSet.Where(s => s.TaskGroup.Id == taskGroupId).Include(s=>s.TaskGroup).ThenInclude(s=>s.User).ToList();
			}
		}

		public IEnumerable<TaskEntity> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return dbContext.TaskSet.Where(s => s.TaskGroup.Id == taskGroupId).Include(s => s.TaskGroup)
					.ThenInclude(s => s.User).Include(s=>s.State).Where(s=>s.State.Name == kanbanStateName)
					.ToList();
			}
		}

		public async Task<IEnumerable<TaskEntity>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken cancellationToken = default)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return await dbContext.TaskSet.Where(s => s.TaskGroup.Id == taskGroupId).Include(s => s.TaskGroup)
					.ThenInclude(s => s.User).Include(s => s.State).Where(s => s.State.Name == kanbanStateName)
					.ToListAsync(cancellationToken);
			}
		}
	}
}