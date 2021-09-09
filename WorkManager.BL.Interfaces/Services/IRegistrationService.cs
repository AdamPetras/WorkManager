using System.Threading.Tasks;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Services
{
	public interface IRegistrationService : IService
	{
		bool RegisterUser(IUserModel model);
		Task<bool> RegisterUserAsync(IUserModel model);
		bool RegisterAndAuthenticateUser(IUserModel model);
		Task<bool> RegisterAndAuthenticateUserAsync(IUserModel model);
	}
}