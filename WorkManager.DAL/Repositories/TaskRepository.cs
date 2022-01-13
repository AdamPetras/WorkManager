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
            return dbSet.ToList();
        }

        protected override void AddInt(TaskEntity entity, WorkManagerDbContext dbContext)
        {
            if (entity.TaskGroup != null)
            {
                dbContext.Entry(entity.State).State = EntityState.Unchanged;
                dbContext.Entry(entity.TaskGroup.User).State = EntityState.Unchanged;
                dbContext.Entry(entity.TaskGroup).State = EntityState.Unchanged;
            }
        }

        public ICollection<TaskEntity> GetTasksByTaskGroupId(Guid taskGroupId)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return dbContext.TaskSet.AsQueryable().Where(s => s.TaskGroup.Id == taskGroupId).ToList();
            }
        }

        public ICollection<TaskEntity> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return dbContext.TaskSet.AsQueryable().Where(s => s.TaskGroupId == taskGroupId).Where(s => s.State.Name == kanbanStateName)
                    .ToList();
            }
        }

        public async Task<ICollection<TaskEntity>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken cancellationToken = default)
        {
            using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                return await dbContext.TaskSet.AsQueryable().Where(s => s.TaskGroup.Id == taskGroupId).Where(s => s.State.Name == kanbanStateName)
                    .ToListAsync(cancellationToken);
            }
        }

        public uint GetTasksCountByTaskGroupId(Guid taskGroupId)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return (uint)dbContext.TaskSet.Count(s => s.TaskGroup.Id == taskGroupId);
            }
        }

        public async Task ClearTasksByKanbanStateAsync(Guid kanbanStateId, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
            {
                foreach (TaskEntity entity in dbContext.TaskSet.AsQueryable().Where(s => s.State.Id == kanbanStateId))
                {
                    dbContext.TaskSet.Attach(entity);
                    dbContext.TaskSet.Remove(entity);
                }
                await dbContext.SaveChangesAsync(token);
            }
        }
    }
}