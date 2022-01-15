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
		public KanbanStateRepository(IDBContextFactory<WorkManagerDbContext> idbContextFactory) : base(idbContextFactory)
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

		public ICollection<KanbanStateEntity> GetKanbanStateByTaskOrderedByStateGroup(Guid taskGroupId)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.KanbanSet.Where(s => s.TaskGroupId == taskGroupId).OrderBy(s => s.StateOrder).ToList();
			}
		}

        public async Task<ICollection<KanbanStateEntity>> GetKanbanStateByTaskGroupOrderedByStateAsync(Guid taskGroupId, CancellationToken token)
        {
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return await dbContext.KanbanSet.Where(s => s.TaskGroupId == taskGroupId).OrderBy(s => s.StateOrder).ToListAsync(token);
            }
		}

        public KanbanStateEntity GetNextKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return dbContext.KanbanSet.SingleOrDefault(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder + 1);
            }
		}

        public async Task<KanbanStateEntity> GetNextKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return await dbContext.KanbanSet.SingleOrDefaultAsync(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder + 1, token);
			}
		}

        public KanbanStateEntity GetPreviousKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return dbContext.KanbanSet.SingleOrDefault(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder - 1);
            }
        }

        public async Task<KanbanStateEntity> GetPreviousKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return await dbContext.KanbanSet.SingleOrDefaultAsync(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder - 1, token);
            }
        }
	}
}