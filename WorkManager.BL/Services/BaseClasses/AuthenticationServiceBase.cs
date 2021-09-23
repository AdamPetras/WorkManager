using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Prism.Events;
using WorkManager.BL.Exceptions;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Services.BaseClasses
{
	public abstract class AuthenticationServiceBase : IAuthenticationService
	{
		private readonly IUserFacade _facade;
		private readonly ICurrentModelProviderManager<IUserModel> _currentUserProviderManager;

		protected const int DefaultSaltLength = 32;
		protected const int DefaultHashLength = 128;
		protected const int DefaultNumberOfPbkdfIterations = 1000;

		protected AuthenticationServiceBase(IUserFacade facade, ICurrentModelProviderManager<IUserModel> currentUserProviderManager)
		{
			_facade = facade;
			_currentUserProviderManager = currentUserProviderManager;
		}

		public IUserModel Authenticate(string username, string password)
		{
			string storedPassword = _facade.GetPasswordByUserName(username);

			if (storedPassword == null)
			{
				_currentUserProviderManager.SetItem(null);
				throw new UnauthorizedAccessException("Failed to authenticate user.");
			}

			if (!PasswordMatchesHashedPassword(password, storedPassword))
			{
				_currentUserProviderManager.SetItem(null);
				throw new UnauthorizedAccessException("Failed to authenticate user.");
			}

			IUserModel user = _facade.GetByUserName(username);

			if (user == null)
			{
				_currentUserProviderManager.SetItem(null);
				throw new UnauthorizedAccessException("Failed to authenticate user.");
			}
			_currentUserProviderManager.SetItem(user);
			return user;
		}

		public async Task<IUserModel> AuthenticateAsync(string username, string password)
		{
			string storedPassword = await _facade.GetPasswordByUserNameAsync(username);

			if (storedPassword == null)
			{
				_currentUserProviderManager.SetItem(null);
				throw new UnauthorizedAccessException("Failed to authenticate user.");
			}

			if (!await PasswordMatchesHashedPasswordAsync(password, storedPassword))
			{
				_currentUserProviderManager.SetItem(null);
				throw new UnauthorizedAccessException("Failed to authenticate user.");
			}

			IUserModel user = await _facade.GetByUserNameAsync(username);

			if (user == null)
			{
				_currentUserProviderManager.SetItem(null);
				throw new UnauthorizedAccessException("Failed to authenticate user.");
			}
			_currentUserProviderManager.SetItem(user);
			return user;
		}

		public string GetHashedPassword(string password)
		{
			var salt = new byte[DefaultSaltLength];
			new RNGCryptoServiceProvider().GetBytes(salt);
			var pbkdf = new Rfc2898DeriveBytes(password, salt, DefaultNumberOfPbkdfIterations);
			var hash = pbkdf.GetBytes(DefaultHashLength);
			var hashWithSalt = new byte[DefaultSaltLength + DefaultHashLength];
			Array.Copy(salt, 0, hashWithSalt, 0, DefaultSaltLength);
			Array.Copy(hash, 0, hashWithSalt, DefaultSaltLength, DefaultHashLength);
			var hashWithSaltString = Convert.ToBase64String(hashWithSalt);
			return hashWithSaltString;
		}

		public async Task<string> GetHashedPasswordAsync(string password)
		{
			return await Task.Run(()=> GetHashedPassword(password));
		}


		protected bool PasswordMatchesHashedPassword(string password, string hashedPassword)
		{
			var hash = Convert.FromBase64String(hashedPassword);
			var salt = new byte[DefaultSaltLength];
			Array.Copy(hash, 0, salt, 0, DefaultSaltLength);
			var pkbdf = new Rfc2898DeriveBytes(password, salt, DefaultNumberOfPbkdfIterations);
			var providedPasswordHash = pkbdf.GetBytes(DefaultHashLength);
			for (int i = 0; i < DefaultHashLength; i++)
			{
				if (hash[i + DefaultSaltLength] != providedPasswordHash[i])
				{
					return false;
				}
			}
			return true;
		}

		public async Task<bool> PasswordMatchesHashedPasswordAsync(string password, string hashedPassword)
		{
			return await Task.Run(()=> PasswordMatchesHashedPassword(password, hashedPassword));
		}
	}
}