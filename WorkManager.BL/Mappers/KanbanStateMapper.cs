using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class KanbanStateMapper : IMapper<KanbanStateEntity, IKanbanStateModel>
	{
		public KanbanStateEntity Map(IKanbanStateModel model)
		{
			if (model == null)
				return new KanbanStateEntity();
			return new KanbanStateEntity()
			{
				Id= model.Id,
				Name = model.Name,
				StateOrder = model.StateOrder,
				IconName= model.IconName
			};
		}

		public IKanbanStateModel Map(KanbanStateEntity entity)
		{
			if (entity == null)
				return new KanbanStateModel();
			return new KanbanStateModel(entity.Id, entity.Name, entity.StateOrder, entity.IconName);
		}
	}
}