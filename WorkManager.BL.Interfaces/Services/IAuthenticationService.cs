using System.Threading;
using System.Threading.Tasks;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Services
{
	public interface IAuthenticationService : IService
	{
		IUserModel Authenticate(string username, string password);
		Task<IUserModel> AuthenticateAsync(string username, string password, CancellationToken token);
        Task LogoutAsync(CancellationToken token);
        string GetHashedPassword(string password);
		Task<string> GetHashedPasswordAsync(string password, CancellationToken token);
        bool PasswordMatchesHashedPassword(string password, string hashedPassword);
        Task<bool> PasswordMatchesHashedPasswordAsync(string password, string hashedPassword, CancellationToken token);
        bool HasPasswordCorrectStructure(string password);
    }
}