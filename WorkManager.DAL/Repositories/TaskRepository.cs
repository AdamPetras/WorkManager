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
	public class TaskRepository : RepositoryBase<TaskEntity>, ITaskRepository
	{
		public TaskRepository(IDBContextFactory<WorkManagerDbContext> idbContextFactory) : base(idbContextFactory)
		{
		}

		protected override ICollection<TaskEntity> GetAllInt(IQueryable<TaskEntity> dbSet)
		{
			return dbSet.Include(s => s.TaskGroup).ToList();
		}

		public override TaskEntity Add(TaskEntity entity)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
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

		public ICollection<TaskEntity> GetTasksByTaskGroupId(Guid taskGroupId)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.TaskSet.Where(s => s.TaskGroup.Id == taskGroupId).Include(s=>s.TaskGroup).ThenInclude(s=>s.User).ToList();
			}
		}

		public ICollection<TaskEntity> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.TaskSet.Where(s => s.TaskGroup.Id == taskGroupId).Include(s => s.TaskGroup)
					.ThenInclude(s => s.User).Include(s=>s.State).Where(s=>s.State.Name == kanbanStateName)
					.ToList();
			}
		}

		public async Task<ICollection<TaskEntity>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken cancellationToken = default)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return await dbContext.TaskSet.Where(s => s.TaskGroup.Id == taskGroupId).Include(s => s.TaskGroup)
					.ThenInclude(s => s.User).Include(s => s.State).Where(s => s.State.Name == kanbanStateName)
					.ToListAsync(cancellationToken);
			}
		}
	}
}