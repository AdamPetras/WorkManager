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
	public class KanbanTaskGroupFacade : FacadeBase<IKanbanTaskGroupModel,KanbanTaskGroupEntity>, IKanbanTaskGroupFacade
	{
		public new IKanbanTaskGroupRepository Repository { get; }

		public KanbanTaskGroupFacade(IKanbanTaskGroupRepository repository, IMapper<KanbanTaskGroupEntity, IKanbanTaskGroupModel> mapper) : base(repository, mapper)
		{
			Repository = repository;
		}

		public ICollection<IKanbanTaskGroupModel> GetKanbansByTaskGroupId(Guid taskGroupId)
		{
			return Repository.GetKanbansByTaskGroupId(taskGroupId).Select(Mapper.Map).ToList();
		}

		public async Task<ICollection<IKanbanTaskGroupModel>> GetKanbansByTaskGroupIdAsync(Guid taskGroupId, CancellationToken token = default)
		{
			return (await Repository.GetKanbansByTaskGroupIdAsync(taskGroupId,token)).Select(Mapper.Map).ToList();
		}
	}
}