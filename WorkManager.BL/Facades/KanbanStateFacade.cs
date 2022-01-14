using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class KanbanStateFacade:FacadeBase<IKanbanStateModel,KanbanStateEntity>, IKanbanStateFacade
	{
		protected new IKanbanStateRepository Repository;
		public KanbanStateFacade(IKanbanStateRepository repository, IMapper<KanbanStateEntity, IKanbanStateModel> mapper) : base(repository, mapper)
		{
			Repository = repository;
		}

		public void CreateDefaultKanbanStateModels(ITaskGroupModel taskGroup)
		{
			Repository.Add(Mapper.Map(new KanbanStateModel(Guid.NewGuid(), "Todo", 0, "\uf46d", taskGroup.Id)));
			Repository.Add(Mapper.Map(new KanbanStateModel(Guid.NewGuid(), "In Progress", 1, "\uf110", taskGroup.Id)));
			Repository.Add(Mapper.Map(new KanbanStateModel(Guid.NewGuid(), "Done", 2, "\uf46c", taskGroup.Id)));
		}

		public IEnumerable<IKanbanStateModel> GetKanbanStatesByTaskGroup(Guid taskGroupId)
		{
			return Repository.GetKanbanStateByTaskOrderedByStateGroup(taskGroupId).Select(Mapper.Map);
		}

        public async IAsyncEnumerable<IKanbanStateModel> GetKanbanStatesByTaskGroupOrderedByStateAsync(Guid taskGroupId, CancellationToken token = default)
        {
            foreach (KanbanStateEntity kanbanStateEntity in await Repository.GetKanbanStateByTaskGroupOrderedByStateAsync(taskGroupId,token))
            {
                yield return await Mapper.MapAsync(kanbanStateEntity, token);
            }
        }

        public IKanbanStateModel GetNextKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            return Mapper.Map(Repository.GetNextKanbanState(taskGroupId, currentStateOrder));

        }

        public async Task<IKanbanStateModel> GetNextKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token = default)
        {
            return await Mapper.MapAsync(await Repository.GetNextKanbanStateAsync(taskGroupId, currentStateOrder, token), token);
        }

        public IKanbanStateModel GetPreviousKanbanState(Guid taskGroupId, int currentStateOrder)
        {
            return Mapper.Map(Repository.GetPreviousKanbanState(taskGroupId, currentStateOrder));
        }

        public async Task<IKanbanStateModel> GetPreviousKanbanStateAsync(Guid taskGroupId, int currentStateOrder, CancellationToken token = default)
        {
            return await Mapper.MapAsync(await Repository.GetPreviousKanbanStateAsync(taskGroupId, currentStateOrder, token), token);
        }
    }
}