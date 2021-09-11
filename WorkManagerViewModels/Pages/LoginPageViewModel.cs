using System;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Prism.Commands;
using Prism.Navigation;
using WorkManager.BL.Exceptions;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class LoginPageViewModel : ViewModelBase
	{
		private readonly IAuthenticationService _authenticationService;
		private readonly IRegistrationService _registrationService;
		private readonly IToastMessageService _toastMessageService;
		private readonly IUserModel _testModel = new UserModel(new Guid("4E6A3273-35DB-4E73-8014-0A3566724B1B"), "", "", "###", "###");


		public LoginPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IRegistrationService registrationService, IToastMessageService toastMessageService) :
			base(navigationService)
		{
			_authenticationService = authenticationService;
			_registrationService = registrationService;
			_toastMessageService = toastMessageService;
			LoginCommand = new DelegateCommand(async()=> await LoginAsync()).ObservesProperty(()=>IsBusy);
			ShowRegisterCommand = new DelegateCommand(Register);
			ContinueWithoutLoginCommand = new DelegateCommand(async ()=>await ContinueWithoutLogin());
		}

		public DelegateCommand ShowRegisterCommand { get; }
		public DelegateCommand ContinueWithoutLoginCommand { get; }
		public DelegateCommand LoginCommand { get; }

		private string _username;
		public string Username
		{
			get => _username;
			set
			{
				if (_username == value) return;
				_username = value;
				RaisePropertyChanged();
			}
		}

		private string _password;
		public string Password
		{
			get => _password;
			set
			{
				if (_password == value) return;
				_password = value;
				RaisePropertyChanged();
			}
		}


		private async void Register()
		{
			await NavigationService.NavigateAsync("RegisterPage");
		}

		private async Task LoginAsync()
		{
			IsBusy = true;
			if(!IsUsernameRightShowDialog() || !IsPasswordRightShowDialog())
			{
				IsBusy = false;
				return;
			}
			try
			{
				IUserModel user = await _authenticationService.AuthenticateAsync(Username, Password);
				if (user != null)
					await NavigationService.NavigateAsync("/RootPage/NavigationPage/TaskGroupPage");
			}
			catch (UnauthorizedAccessException)
			{
				_toastMessageService.LongAlert(LoginPageViewModelSR.UnauthorizedAccessExceptionMessage);
			}
			finally
			{
				IsBusy = false;
			}
		}

		private bool IsPasswordRightShowDialog()
		{
			if (Password.IsNullOrEmpty())
			{
				_toastMessageService.LongAlert(LoginPageViewModelSR.PasswordIsNullOrEmpty);
				return false;
			}
			return true;
		}

		private bool IsUsernameRightShowDialog()
		{
			if (Username.IsNullOrEmpty())
			{
				_toastMessageService.LongAlert(LoginPageViewModelSR.UserNameIsNullOrEmpty);
				return false;
			}
			return true;
		}

		private async Task ContinueWithoutLogin()
		{
			IsBusy = true;
			try
			{
				await _registrationService.RegisterAndAuthenticateUserAsync(_testModel);
			}
			catch (UserAlreadyExistsException)
			{
				if (await _authenticationService.AuthenticateAsync(_testModel.Username, _testModel.Password) != null)
					NavigationService.NavigateAsync("/RootPage/NavigationPage/TaskGroupPage");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}