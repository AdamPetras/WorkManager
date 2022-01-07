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
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class TaskFacade : FacadeBase<ITaskModel,TaskEntity>, ITaskFacade
	{
		protected new ITaskRepository Repository;
		public TaskFacade(ITaskRepository repository, IMapper<TaskEntity, ITaskModel> mapper) : base(repository, mapper)
		{
			Repository = repository;
		}

		public IEnumerable<ITaskModel> GetTasksByTaskGroupId(Guid taskGroupId)
		{
			return Repository.GetTasksByTaskGroupId(taskGroupId).Select(Mapper.Map);
		}

		public IEnumerable<ITaskModel> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName)
		{
			return Repository.GetTasksByTaskGroupIdAndKanbanState(taskGroupId,kanbanStateName).Select(Mapper.Map);
		}

		public async Task<IEnumerable<ITaskModel>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken cancellationToken = default)
		{
			return (await Repository.GetTasksByTaskGroupIdAndKanbanStateAsync(taskGroupId, kanbanStateName, cancellationToken)).Select(Mapper.Map);
		}

        public Task ClearTasksByKanbanStateAsync(Guid kanbanStateId, CancellationToken token = default)
        {
			return Repository.ClearTasksByKanbanStateAsync(kanbanStateId, token);

		}
	}
}