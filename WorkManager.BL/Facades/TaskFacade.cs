using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class TaskFacade : FacadeBase<ITaskModel,TaskEntity>, ITaskFacade
	{
        protected new readonly ITaskMapper Mapper;

        public TaskFacade(WorkManagerDbContext dbContext, ITaskMapper mapper,
            IDatabaseSessionController databaseSessionController) : base(dbContext, mapper, databaseSessionController)
        {
            Mapper = mapper;
        }

        public async Task<ITaskModel> GetByIdAsync(Guid taskId, CancellationToken token)
        {
            DatabaseSessionController.Reset();
            TaskEntity entity = await DbContext.TaskSet.Include(s => s.RelatedTask).SingleOrDefaultAsync(s => s.Id == taskId, token);
            return Mapper.Map(entity, await GetImagesCountAsync(entity.Id, token));
        }

        public ICollection<ITaskModel> GetTasksByTaskGroupId(Guid taskGroupId)
		{
            DatabaseSessionController.Reset();
			return DbContext.TaskSet.AsQueryable().Where(s=>s.TaskGroupId == taskGroupId).ToList().Select(s => Mapper.Map(s, GetImagesCount(s.Id))).ToList();
		}

        public async Task<ICollection<ITaskModel>> GetTasksByTaskGroupIdAsync(Guid taskGroupId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			return await (await DbContext.TaskSet.AsQueryable().Where(s=>s.TaskGroupId == taskGroupId).ToListAsync(token)).ToAsyncEnumerable().SelectAwait( async s => Mapper.Map(s, await GetImagesCountAsync(s.Id, token))).ToListAsync(token);
        }

        public ICollection<ITaskModel> GetTasksByTaskGroupNoRelatedToTask(ITaskModel task, Guid taskGroupId)
        {
            DatabaseSessionController.Reset();
            return DbContext.TaskSet.Include(s=>s.RelatedTask).AsQueryable().Where(t => t.TaskGroupId == taskGroupId && t.RelatedTask.RelatedTasks.All(s => s.Id != task.Id)).ToList().Select(s => Mapper.Map(s, GetImagesCount(s.Id))).ToList();
        }

        public async Task<ICollection<ITaskModel>> GetTasksByTaskGroupNoRelatedToTaskAsync(ITaskModel task, Guid taskGroupId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await DbContext.TaskSet.Include(s => s.RelatedTask).AsQueryable()
                .Where(t => t.TaskGroupId == taskGroupId && t.RelatedTask.RelatedTasks.All(s => s.Id != task.Id))
                .ToList().ToAsyncEnumerable().SelectAwait(async s => Mapper.Map(s, await GetImagesCountAsync(s.Id, token)))
                .ToListAsync(token);
        }

        public ICollection<ITaskModel> GetTasksByTaskGroupIdAndKanbanState(Guid taskGroupId, string kanbanStateName)
		{
            DatabaseSessionController.Reset();
			return DbContext.TaskSet.AsQueryable().Where(s => s.TaskGroupId == taskGroupId && s.State.Name == kanbanStateName).ToList().Select(s => Mapper.Map(s, GetImagesCount(s.Id))).ToList();
		}

		public async Task<ICollection<ITaskModel>> GetTasksByTaskGroupIdAndKanbanStateAsync(Guid taskGroupId, string kanbanStateName, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			return await (await DbContext.TaskSet.AsQueryable().Where(s=>s.TaskGroupId == taskGroupId && s.State.Name == kanbanStateName).ToListAsync(token)).ToAsyncEnumerable().SelectAwait(async s => Mapper.Map(s, await GetImagesCountAsync(s.Id, token))).ToListAsync(token);
        }

        public async Task ClearTasksByKanbanStateAsync(Guid kanbanStateId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            foreach (TaskEntity entity in DbContext.TaskSet.AsQueryable().Where(s => s.State.Id == kanbanStateId))
            {
                DbContext.TaskSet.Attach(entity);
                DbContext.TaskSet.Remove(entity);
            }
            await DbContext.SaveChangesAsync(token).ConfigureAwait(false);
        }

        private int GetImagesCount(Guid taskId) => DbContext.ImageSet.Count(s => s.TaskId == taskId);
        private Task<int> GetImagesCountAsync(Guid taskId, CancellationToken token) => DbContext.ImageSet.AsQueryable().CountAsync(s => s.TaskId == taskId, token);
    }
}