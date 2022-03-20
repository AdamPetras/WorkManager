using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
    public interface IKanbanStateMapper : IMapper<KanbanStateEntity, IKanbanStateModel>
    {
        IKanbanStateModel Map(KanbanStateEntity entity);
    }
}