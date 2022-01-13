using System.Linq;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
    public class TaskGroupMapper : IMapper<TaskGroupEntity, ITaskGroupModel>
    {
        private readonly IMapper<UserEntity, IUserModel> _userMapper;
        private readonly ITaskRepository _taskRepository;

        public TaskGroupMapper(IMapper<UserEntity, IUserModel> userMapper, ITaskRepository taskRepository)
        {
            _userMapper = userMapper;
            _taskRepository = taskRepository;
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

        public ITaskGroupModel Map(TaskGroupEntity entity)
        {
            if (entity == null)
                return new TaskGroupModel();
            return new TaskGroupModel(entity.Id, entity.Name, entity.Description, _taskRepository.GetTasksCountByTaskGroupId(entity.Id), entity.UserId);
        }
    }
}