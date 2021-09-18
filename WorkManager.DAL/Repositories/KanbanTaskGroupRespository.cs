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
	public class KanbanTaskGroupRespository : RepositoryBase<KanbanTaskGroupEntity>, IKanbanTaskGroupRepository
	{
		public KanbanTaskGroupRespository(IDBContextFactory<WorkManagerDbContext> idbContextFactory) : base(idbContextFactory)
		{
		}

		protected override IEnumerable<KanbanTaskGroupEntity> GetAllInt(IQueryable<KanbanTaskGroupEntity> dbSet)
		{
			throw new System.NotImplementedException();
		}

		protected override void AddInt(KanbanTaskGroupEntity entity, WorkManagerDbContext dbContext)
		{
			if (entity.TaskGroup != null)
			{
				dbContext.Entry(entity.TaskGroup).State = EntityState.Unchanged;
				if(entity.TaskGroup.User != null)
				dbContext.Entry(entity.TaskGroup.User).State = EntityState.Unchanged;
			}
			if (entity.Kanban != null)
				dbContext.Entry(entity.Kanban).State = EntityState.Unchanged;									
		}

		public IEnumerable<KanbanTaskGroupEntity> GetKanbansByTaskGroupId(Guid taskGroupId)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.KanbanTaskGroupSet.Where(s => s.IdTaskGroup == taskGroupId).Include(s=>s.Kanban).Include(s=>s.TaskGroup).ToList();
			}
		}

		public async Task<IEnumerable<KanbanTaskGroupEntity>> GetKanbansByTaskGroupIdAsync(Guid taskGroupId, CancellationToken token = default)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return await dbContext.KanbanTaskGroupSet.Where(s => s.IdTaskGroup == taskGroupId).Include(s => s.Kanban).Include(s => s.TaskGroup).ToListAsync(token);
			}
		}
	}
}