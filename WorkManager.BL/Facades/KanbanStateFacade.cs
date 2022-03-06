using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class KanbanStateFacade:FacadeBase<IKanbanStateModel,KanbanStateEntity>, IKanbanStateFacade
	{
		protected new IKanbanStateRepository Repository;
		public KanbanStateFacade(IKanbanStateRepository repository,
            IMapper<KanbanStateEntity, IKanbanStateModel> mapper, IDatabaseSessionController databaseSessionController) : base(repository, mapper, databaseSessionController)
		{
			Repository = repository;
		}

		public void CreateDefaultKanbanStateModels(ITaskGroupModel taskGroup)
		{
			Repository.Add(Mapper.Map(new KanbanStateModel(Guid.NewGuid(), "Todo", 0, "\uf46d", taskGroup.Id)));
			Repository.Add(Mapper.Map(new KanbanStateModel(Guid.NewGuid(), "In Progress", 1, "\uf110", taskGroup.Id)));
			Repository.Add(Mapper.Map(new KanbanStateModel(Guid.NewGuid(), "Done", 2, "\uf46c", taskGroup.Id)));
		}

		public ICollection<IKanbanStateModel> GetKanbanStatesByTaskGroup(Guid taskGroupId)
		{
            DatabaseSessionController.Reset();
			return Repository.GetWhere(s=>s.TaskGroupId == taskGroupId).Select(Mapper.Map).ToList();
		}

        public async Task<ICollection<IKanbanStateModel>> GetKanbanStatesByTaskGroupOrderedByStateAsync(Guid taskGroupId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			return (await Repository.GetWhereOrderByAsync(s=>s.TaskGroupId == taskGroupId, s => s.StateOrder, token).ConfigureAwait(false)).Select(Mapper.Map).ToList();
        }

        public IKanbanStateModel GetNextKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(Repository.GetNextKanbanState(taskGroupId, currentStateOrder));
        }

        public async Task<IKanbanStateModel> GetNextKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await Mapper.MapAsync(await Repository.GetNextKanbanStateAsync(taskGroupId, currentStateOrder, token).ConfigureAwait(false), token).ConfigureAwait(false);
        }

        public IKanbanStateModel GetPreviousKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(Repository.GetPreviousKanbanState(taskGroupId, currentStateOrder));
        }

        public async Task<IKanbanStateModel> GetPreviousKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await Mapper.MapAsync(await Repository.GetPreviousKanbanStateAsync(taskGroupId, currentStateOrder, token).ConfigureAwait(false), token).ConfigureAwait(false);
        }
    }
}