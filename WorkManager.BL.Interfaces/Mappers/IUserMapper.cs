using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
    public interface IUserMapper : IMapper<UserEntity, IUserModel>
    {
        IUserModel Map(UserEntity entity);
    }
}