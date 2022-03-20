using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class KanbanStateFacade:FacadeBase<IKanbanStateModel,KanbanStateEntity>, IKanbanStateFacade
	{
        protected new readonly IKanbanStateMapper Mapper;

        public KanbanStateFacade(WorkManagerDbContext dbContext, IKanbanStateMapper mapper, IDatabaseSessionController databaseSessionController) : base(dbContext, mapper, databaseSessionController)
        {
            Mapper = mapper;
        }

		public void CreateDefaultKanbanStateModels(ITaskGroupModel taskGroup)
		{
			DbContext.KanbanSet.Add(Mapper.Map(new KanbanStateModel(Guid.NewGuid(), "Todo", 0, "\uf46d", taskGroup.Id)));
            DbContext.KanbanSet.Add(Mapper.Map(new KanbanStateModel(Guid.NewGuid(), "In Progress", 1, "\uf110", taskGroup.Id)));
            DbContext.KanbanSet.Add(Mapper.Map(new KanbanStateModel(Guid.NewGuid(), "Done", 2, "\uf46c", taskGroup.Id)));
		}

		public ICollection<IKanbanStateModel> GetKanbanStatesByTaskGroup(Guid taskGroupId)
		{
            DatabaseSessionController.Reset();
			return DbContext.KanbanSet.AsQueryable().Where(s=>s.TaskGroupId == taskGroupId).AsEnumerable().Select(Mapper.Map).ToList();
		}

        public async Task<ICollection<IKanbanStateModel>> GetKanbanStatesByTaskGroupOrderedByStateAsync(Guid taskGroupId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			return await DbContext.KanbanSet.AsQueryable().Where(s => s.TaskGroupId == taskGroupId).OrderBy(s=>s.StateOrder).ToAsyncEnumerable().Select(Mapper.Map).ToListAsync(token);
        }

        public IKanbanStateModel GetNextKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(DbContext.KanbanSet.SingleOrDefault(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder + 1));
        }

        public async Task<IKanbanStateModel> GetNextKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(await DbContext.KanbanSet.AsQueryable()
                .SingleOrDefaultAsync(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder + 1, token)
                .ConfigureAwait(false));
        }

        public IKanbanStateModel GetPreviousKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(DbContext.KanbanSet.SingleOrDefault(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder - 1));
        }

        public async Task<IKanbanStateModel> GetPreviousKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(await DbContext.KanbanSet.AsQueryable().SingleOrDefaultAsync(s => s.TaskGroupId == taskGroupId && s.StateOrder == currentStateOrder - 1, token).ConfigureAwait(false));
        }
    }
}