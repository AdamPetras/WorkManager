using System.Threading.Tasks;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IUserRepository: IRepository<UserEntity>
	{
		UserEntity GetByUserName(string username);
		Task<UserEntity> GetByUserNameAsync(string username);
		string GetPasswordByUserName(string username);
		Task<string> GetPasswordByUserNameAsync(string username);
		bool Exists(string username);
		Task<bool> ExistsAsync(string username);
    }
}