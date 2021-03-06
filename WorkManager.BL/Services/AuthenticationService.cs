using Prism.Navigation;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.Services.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Services
{
	public class AuthenticationService: AuthenticationServiceBase
	{
		public AuthenticationService(IUserFacade facade, ICurrentModelProviderManager<IUserModel> currentUserProviderManager, INavigationService navigationService, IDatabaseSessionController databaseSessionController) 
            : base(facade, currentUserProviderManager,navigationService, databaseSessionController)
		{
		}
	}
}