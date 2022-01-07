using System.Linq;
using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
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
				IdTaskGroup = model.TaskGroup.Id,
				TaskGroup = _taskGroupMapper.Map(model.TaskGroup),
				IdState = model.State.Id,
				State = _kanbanMapper.Map(model.State),
				Priority = model.Priority,
				WorkTime = model.WorkTime,
			};
		}

		public ITaskModel Map(TaskEntity entity)
		{
			if (entity == null)
				return new TaskModel();
			return new TaskModel(entity.Id, entity.ActualDateTime, entity.Name, _imageRepository.GetImagesCountByTask(entity.Id), entity.Description,
				entity.TaskDoneDateTime, _taskGroupMapper.Map(entity.TaskGroup), _kanbanMapper.Map(entity.State), entity.Priority,entity.WorkTime);
		}
	}
}