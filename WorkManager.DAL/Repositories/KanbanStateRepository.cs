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

		protected override ICollection<KanbanStateEntity> GetAllInt(IQueryable<KanbanStateEntity> dbSet)
		{
			return dbSet.ToList();
		}

		protected override void AddInt(KanbanStateEntity entity, WorkManagerDbContext dbContext)
		{
			if (entity.TaskGroup != null)
			{
				dbContext.Entry(entity.TaskGroup.User).State = EntityState.Unchanged;
				dbContext.Entry(entity.TaskGroup).State = EntityState.Unchanged;
			}
		}

		public ICollection<KanbanStateEntity> GetKanbanStateByTaskGroup(Guid taskGroupId)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.KanbanSet.Include(s => s.TaskGroup).Where(s => s.IdTaskGroup == taskGroupId).ToList();
			}
		}

        public async Task<ICollection<KanbanStateEntity>> GetKanbanStateByTaskGroupAsync(Guid taskGroupId, CancellationToken token)
        {
			using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
            {
                return await dbContext.KanbanSet.Include(s => s.TaskGroup).Where(s => s.IdTaskGroup == taskGroupId).ToListAsync(token);
            }
		}
    }
}