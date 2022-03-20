using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
    public interface IRelatedTaskMapper : IMapper<RelatedTaskEntity, IRelatedTaskModel>
    {
        IRelatedTaskModel Map(RelatedTaskEntity entity);
    }
}