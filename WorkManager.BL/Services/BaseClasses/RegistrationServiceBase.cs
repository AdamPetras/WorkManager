using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using WorkManager.BL.Exceptions;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Services.BaseClasses
{
	public abstract class RegistrationServiceBase : IRegistrationService
	{
		protected readonly IUserFacade Facade;
		protected readonly IAuthenticationService AuthenticationService;

		protected RegistrationServiceBase(IUserFacade facade, IAuthenticationService authenticationService)
		{
			Facade = facade;
			AuthenticationService = authenticationService;
		}

		public bool RegisterUser(IUserModel model)
		{
			model.Password = AuthenticationService.GetHashedPassword(model.Password);
			return Facade.Add(model) != null;
		}

		public async Task<bool> RegisterUserAsync(IUserModel model, CancellationToken token)
		{
			model.Password = await AuthenticationService.GetHashedPasswordAsync(model.Password, token);
			return await Facade.AddAsync(model, token) != null;
		}

		public bool RegisterAndAuthenticateUser(IUserModel model)
		{
			string nonHashedPassword = model.Password;
			if (!RegisterUser(model))
				return false;
			if (!Facade.Exists(model.Username))
			{
				throw new UserNotExistsException();
			}
			AuthenticationService.Authenticate(model.Username, nonHashedPassword);
			return true;
		}

		public async Task<bool> RegisterAndAuthenticateUserAsync(IUserModel model, CancellationToken token)
		{
			string nonHashedPassword = model.Password;
			if (await Facade.ExistsAsync(model.Username, token))
			{
				throw new UserAlreadyExistsException();
			}
			if (!await RegisterUserAsync(model, token))
				return false;
			model.Password = nonHashedPassword;
			if (!await Facade.ExistsAsync(model.Username, token))
			{
				throw new UserNotExistsException("Problem with create user.");
			}
			await AuthenticationService.AuthenticateAsync(model.Username, model.Password, token);
			return true;
		}
	}
}