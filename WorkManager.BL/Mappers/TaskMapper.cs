using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class TaskMapper : IMapper<TaskEntity, ITaskModel>
	{
		private readonly IMapper<TaskGroupEntity, ITaskGroupModel> _taskGroupMapper;
		private readonly IMapper<KanbanStateEntity, IKanbanStateModel> _kanbanMapper;
        private readonly IImageRepository _imageRepository;

        public TaskMapper(IMapper<TaskGroupEntity,ITaskGroupModel> taskGroupMapper,IMapper<KanbanStateEntity, IKanbanStateModel> kanbanMapper, IImageRepository imageRepository)
		{
			_taskGroupMapper = taskGroupMapper;
			_kanbanMapper = kanbanMapper;
            _imageRepository = imageRepository;
        }

		public TaskEntity Map(ITaskModel model)
		{
			if (model == null)
				return new TaskEntity();
			return new TaskEntity()
			{
				Id = model.Id,
				ActualDateTime = model.ActualDateTime,
				Name = model.Name,
				Description = model.Description,
				TaskDoneDateTime = model.TaskDoneDateTime,
				TaskGroupId = model.TaskGroupId,
				StateId = model.StateId,
				Priority = model.Priority.GetValue<EPriority>(),
				WorkTime = model.WorkTime,
			};
		}

		public ITaskModel Map(TaskEntity entity)
		{
			if (entity == null)
				return new TaskModel();
			return new TaskModel(entity.Id, entity.ActualDateTime, entity.Name, _imageRepository.Count(s=>s.TaskId == entity.Id), entity.Description,
				entity.TaskDoneDateTime, entity.TaskGroupId, entity.StateId, entity.Priority,entity.WorkTime);
		}

        public Task<TaskEntity> MapAsync(ITaskModel model, CancellationToken token)
        {
			if (model == null)
                return Task.FromResult(new TaskEntity());
            return Task.FromResult(new TaskEntity
            {
                Id = model.Id,
                ActualDateTime = model.ActualDateTime,
                Name = model.Name,
                Description = model.Description,
                TaskDoneDateTime = model.TaskDoneDateTime,
                TaskGroupId = model.TaskGroupId,
                StateId = model.StateId,
                Priority = model.Priority.GetValue<EPriority>(),
                WorkTime = model.WorkTime,
            });
		}

        public async Task<ITaskModel> MapAsync(TaskEntity entity, CancellationToken token)
        {
			if (entity == null)
                return await Task.FromResult<ITaskModel>(new TaskModel());
            return await Task.FromResult<ITaskModel>(new TaskModel(entity.Id, entity.ActualDateTime, entity.Name, await _imageRepository.CountAsync(s=>s.TaskId == entity.Id, token), entity.Description,
                entity.TaskDoneDateTime, entity.TaskGroupId, entity.StateId, entity.Priority, entity.WorkTime));
		}
    }
}