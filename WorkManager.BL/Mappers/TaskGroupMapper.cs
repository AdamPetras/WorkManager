using WorkManager.BL.Interfaces.Mappers;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
    public class TaskGroupMapper : ITaskGroupMapper
    {

        public TaskGroupMapper()
        {
        }

        public TaskGroupEntity Map(ITaskGroupModel model)
        {
            if (model == null)
                return new TaskGroupEntity();
            return new TaskGroupEntity()
            {
                Id = model.Id,
                Description = model.Description,
                Name = model.Name,
                UserId = model.UserId,
            };
        }

        public ITaskGroupModel Map(TaskGroupEntity entity, int tasksCount)
        {
            if (entity == null)
                return new TaskGroupModel();
            return new TaskGroupModel(entity.Id, entity.Name, entity.Description, tasksCount, entity.UserId);
        }
    }
}