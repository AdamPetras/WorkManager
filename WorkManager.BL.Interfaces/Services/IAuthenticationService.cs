using System.Threading.Tasks;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Services
{
	public interface IAuthenticationService : IService
	{
		IUserModel Authenticate(string username, string password);
		Task<IUserModel> AuthenticateAsync(string username, string password);
        void Logout();
        string GetHashedPassword(string password);
		Task<string> GetHashedPasswordAsync(string password);
        bool PasswordMatchesHashedPassword(string password, string hashedPassword);
        Task<bool> PasswordMatchesHashedPasswordAsync(string password, string hashedPassword);
        bool HasPasswordCorrectStructure(string password);
    }
}