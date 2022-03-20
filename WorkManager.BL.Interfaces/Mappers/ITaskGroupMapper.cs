using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
    public interface ITaskGroupMapper : IMapper<TaskGroupEntity, ITaskGroupModel>
    {
        ITaskGroupModel Map(TaskGroupEntity entity, int tasksCount);
    }
}