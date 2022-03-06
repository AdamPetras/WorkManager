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
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class TaskFacade : FacadeBase<ITaskModel,TaskEntity>, ITaskFacade
	{
		protected new ITaskRepository Repository;
		public TaskFacade(ITaskRepository repository, IMapper<TaskEntity, ITaskModel> mapper,
            IDatabaseSessionController databaseSessionController) : base(repository, mapper, databaseSessionController)
		{
			Repository = repository;
		}

		public ICollection<ITaskModel> GetTasksByTaskGroupId(Guid taskGroupId)
		{
            DatabaseSessionController.Reset();
			return Repository.GetWhere(s=>s.TaskGroupId == taskGroupId).Select(Mapper.Map).ToList();
		}

		public ICollection<ITaskModel> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName)
		{
            DatabaseSessionController.Reset();
			return Repository.GetWhere(s => s.TaskGroupId == taskGroupId && s.State.Name == kanbanStateName).Select(Mapper.Map).ToList();
		}

		public async Task<ICollection<ITaskModel>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			return (await Repository.GetWhereAsync(s=>s.TaskGroupId == taskGroupId && s.State.Name == kanbanStateName, token).ConfigureAwait(false)).Select(Mapper.Map).ToList();
        }

        public async Task ClearTasksByKanbanStateAsync(Guid kanbanStateId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			await Repository.ClearTasksByKanbanStateAsync(kanbanStateId, token).ConfigureAwait(false);
        }
	}
}