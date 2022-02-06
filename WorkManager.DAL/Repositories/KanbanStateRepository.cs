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
	public class KanbanStateRepository:RepositoryBase<KanbanStateEntity>, IKanbanStateRepository
	{
		public KanbanStateRepository(WorkManagerDbContext dbContext) : base(dbContext)
		{
		}

		protected override void AddInt(KanbanStateEntity entity, WorkManagerDbContext dbContext)
		{
			if (entity.TaskGroup != null)
			{
				dbContext.Entry(entity.TaskGroup.User).State = EntityState.Unchanged;
				dbContext.Entry(entity.TaskGroup).State = EntityState.Unchanged;
			}
		}

        public KanbanStateEntity GetNextKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            return DbContext.KanbanSet.SingleOrDefault(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder + 1);
        }

        public async Task<KanbanStateEntity> GetNextKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token)
        {
            return await DbContext.KanbanSet.AsQueryable().SingleOrDefaultAsync(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder + 1, token).ConfigureAwait(false);
        }

        public KanbanStateEntity GetPreviousKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            return DbContext.KanbanSet.SingleOrDefault(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder - 1);
        }

        public async Task<KanbanStateEntity> GetPreviousKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token)
        {
            return await DbContext.KanbanSet.AsQueryable().SingleOrDefaultAsync(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder - 1, token).ConfigureAwait(false);
        }
	}
}