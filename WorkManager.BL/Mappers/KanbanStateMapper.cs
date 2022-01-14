using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class KanbanStateMapper : IMapper<KanbanStateEntity, IKanbanStateModel>
	{
		private readonly IMapper<TaskGroupEntity, ITaskGroupModel> _taskGroupMapper;

		public KanbanStateMapper(IMapper<TaskGroupEntity,ITaskGroupModel> taskGroupMapper)
		{
			_taskGroupMapper = taskGroupMapper;
		}

		public KanbanStateEntity Map(IKanbanStateModel model)
		{
			if (model == null)
				return new KanbanStateEntity();
			return new KanbanStateEntity()
			{
				Id= model.Id,
				Name = model.Name,
				StateOrder = model.StateOrder,
				IconName= model.IconName,
				TaskGroupId = model.TaskGroupId,
			};
		}

		public IKanbanStateModel Map(KanbanStateEntity entity)
		{
			if (entity == null)
				return new KanbanStateModel();
			return new KanbanStateModel(entity.Id, entity.Name, entity.StateOrder, entity.IconName, entity.TaskGroupId);
		}

        public Task<KanbanStateEntity> MapAsync(IKanbanStateModel model, CancellationToken token)
        {
			if (model == null)
                return Task.FromResult(new KanbanStateEntity());
            return Task.FromResult(new KanbanStateEntity()
            {
                Id = model.Id,
                Name = model.Name,
                StateOrder = model.StateOrder,
                IconName = model.IconName,
                TaskGroupId = model.TaskGroupId,
            });
		}

        public Task<IKanbanStateModel> MapAsync(KanbanStateEntity entity, CancellationToken token)
        {
			if (entity == null)
                return Task.FromResult<IKanbanStateModel>(new KanbanStateModel());
            return Task.FromResult<IKanbanStateModel>(new KanbanStateModel(entity.Id, entity.Name, entity.StateOrder, entity.IconName, entity.TaskGroupId));
		}
    }
}