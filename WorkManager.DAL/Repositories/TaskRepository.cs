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
        public TaskRepository(WorkManagerDbContext dbContext) : base(dbContext)
        {
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

        public async Task ClearTasksByKanbanStateAsync(Guid kanbanStateId, CancellationToken token)
        {
            foreach (TaskEntity entity in DbContext.TaskSet.AsQueryable().Where(s => s.State.Id == kanbanStateId))
            {
                DbContext.TaskSet.Attach(entity);
                DbContext.TaskSet.Remove(entity);
            }
            await DbContext.SaveChangesAsync(token).ConfigureAwait(false);
        }
    }
}