using System.Linq;
using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class UserMapper: IMapper<UserEntity,IUserModel>
	{

		public UserMapper()
		{
		}

		public UserEntity Map(IUserModel model)
		{
			if (model == null)
				return new UserEntity();
			return new UserEntity()
			{
				Id = model.Id,
				Username = model.Username,
				Password = model.Password,
				FirstName = model.FirstName,
				Surname = model.Surname,
			};
		}

		public IUserModel Map(UserEntity entity)
		{
			if (entity == null)
				return new UserModel();
			return new UserModel(entity.Id, entity.FirstName, entity.Surname, entity.Username, entity.Password);
		}
	}
}