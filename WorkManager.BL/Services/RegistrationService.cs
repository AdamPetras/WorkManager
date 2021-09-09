using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.Services.BaseClasses;

namespace WorkManager.BL.Services
{
	public class RegistrationService : RegistrationServiceBase
	{
		public RegistrationService(IUserFacade facade, IAuthenticationService authenticationService) : base(facade, authenticationService)
		{
		}
	}
}