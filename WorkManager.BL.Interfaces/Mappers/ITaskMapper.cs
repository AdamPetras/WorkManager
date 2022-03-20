using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
    public interface ITaskMapper : IMapper<TaskEntity, ITaskModel>
    {
        ITaskModel Map(TaskEntity entity, int imagesCount);
    }
}