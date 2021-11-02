using System.Linq;
using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class TaskGroupMapper : IMapper<TaskGroupEntity,ITaskGroupModel>
	{
		private readonly IMapper<UserEntity, IUserModel> _userMapper;

		public TaskGroupMapper(IMapper<UserEntity, IUserModel> userMapper)
		{
			_userMapper = userMapper;
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
				IdUser = model.User.Id,
				User = _userMapper.Map(model.User),
			};
		}

		public ITaskGroupModel Map(TaskGroupEntity entity)
		{
			if (entity == null)
				return new TaskGroupModel();
			return new TaskGroupModel(entity.Id, entity.Name,entity.Description, _userMapper.Map(entity.User));
		}
	}
}