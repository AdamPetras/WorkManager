﻿using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class TaskMapper : IMapper<TaskEntity, ITaskModel>
	{
		private readonly IMapper<TaskGroupEntity, ITaskGroupModel> _taskGroupMapper;
		private readonly IMapper<KanbanStateEntity, IKanbanStateModel> _kanbanMapper;

		public TaskMapper(IMapper<TaskGroupEntity,ITaskGroupModel> taskGroupMapper,IMapper<KanbanStateEntity, IKanbanStateModel> kanbanMapper)
		{
			_taskGroupMapper = taskGroupMapper;
			_kanbanMapper = kanbanMapper;
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
				State = _kanbanMapper.Map(model.State)
			};
		}

		public ITaskModel Map(TaskEntity entity)
		{
			if (entity == null)
				return new TaskModel();
			return new TaskModel(entity.Id, entity.ActualDateTime, entity.Name, entity.Description,
				entity.TaskDoneDateTime, _taskGroupMapper.Map(entity.TaskGroup), _kanbanMapper.Map(entity.State));
		}
	}
}