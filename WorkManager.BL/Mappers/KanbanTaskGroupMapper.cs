using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class KanbanTaskGroupMapper : IMapper<KanbanTaskGroupEntity, IKanbanTaskGroupModel>
	{
		private readonly IMapper<TaskGroupEntity, ITaskGroupModel> _taskGroupMapper;
		private readonly IMapper<KanbanStateEntity, IKanbanStateModel> _kanbanMapper;

		public KanbanTaskGroupMapper(IMapper<TaskGroupEntity,ITaskGroupModel> taskGroupMapper, IMapper<KanbanStateEntity, IKanbanStateModel> kanbanMapper)
		{
			_taskGroupMapper = taskGroupMapper;
			_kanbanMapper = kanbanMapper;
		}

		public KanbanTaskGroupEntity Map(IKanbanTaskGroupModel model)
		{
			if (model == null)
				return new KanbanTaskGroupEntity();
			return new KanbanTaskGroupEntity()
			{
				Id = model.Id,
				IdKanban = model.Kanban.Id,
				IdTaskGroup = model.TaskGroup.Id,
				Kanban = _kanbanMapper.Map(model.Kanban),
				TaskGroup = _taskGroupMapper.Map(model.TaskGroup)
			};
		}

		public IKanbanTaskGroupModel Map(KanbanTaskGroupEntity entity)
		{
			if (entity == null)
				return new KanbanTaskGroupModel();
			return new KanbanTaskGroupModel(entity.Id, _taskGroupMapper.Map(entity.TaskGroup),
				_kanbanMapper.Map(entity.Kanban));
		}
	}
}