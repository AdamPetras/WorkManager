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
			return Repository.GetKanbanStateByTaskGroup(taskGroupId).Select(Mapper.Map);
		}

        public async Task<IEnumerable<IKanbanStateModel>> GetKanbanStatesByTaskGroupAsync(Guid taskGroupId, CancellationToken token = default)
        {
			return (await Repository.GetKanbanStateByTaskGroupAsync(taskGroupId, token)).Select(Mapper.Map);
        }
	}
}