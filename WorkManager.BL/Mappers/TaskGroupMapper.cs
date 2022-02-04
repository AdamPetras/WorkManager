using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            return new TaskGroupModel(entity.Id, entity.Name, entity.Description, _taskRepository.Count(s => s.TaskGroupId == entity.Id), entity.UserId);
        }

        public Task<TaskGroupEntity> MapAsync(ITaskGroupModel model, CancellationToken token)
        {
            if (model == null)
                return Task.FromResult(new TaskGroupEntity());
            return Task.FromResult(new TaskGroupEntity()
            {
                Id = model.Id,
                Description = model.Description,
                Name = model.Name,
                UserId = model.UserId,
            });
        }

        public async Task<ITaskGroupModel> MapAsync(TaskGroupEntity entity, CancellationToken token)
        {
            if (entity == null)
                return new TaskGroupModel();
            return new TaskGroupModel(entity.Id, entity.Name, entity.Description, await _taskRepository.CountAsync(s=>s.TaskGroupId == entity.Id,token), entity.UserId);
        }
    }
}