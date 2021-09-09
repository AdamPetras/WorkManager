using System.Threading.Tasks;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Services
{
	public interface IAuthenticationService : IService
	{
		public IUserModel Authenticate(string username, string password);
		public Task<IUserModel> AuthenticateAsync(string username, string password);
		public string GetHashedPassword(string password);
		public Task<string> GetHashedPasswordAsync(string password);
	}
}