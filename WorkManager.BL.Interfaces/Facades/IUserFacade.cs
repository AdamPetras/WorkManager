using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface IUserFacade : IFacade<IUserModel>
	{
		IUserModel GetByUserName(string username);
		Task<IUserModel> GetByUserNameAsync(string username, CancellationToken token = default);
		string GetPasswordByUserName(string username);
		Task<string> GetPasswordByUserNameAsync(string username, CancellationToken token = default);
		bool Exists(string username);
		Task<bool> ExistsAsync(string username, CancellationToken token = default);
    }
}