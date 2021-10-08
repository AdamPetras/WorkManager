using System;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Prism.Commands;
using Prism.Navigation;
using WorkManager.BL.Exceptions;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.Services;
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
		private readonly WorkManagerSettingsService _workManagerSettingsService;
		private readonly IUserModel _testModel = new UserModel(new Guid("4E6A3273-35DB-4E73-8014-0A3566724B1B"), "", "", "###", "###");


		public LoginPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService, IRegistrationService registrationService, IToastMessageService toastMessageService,
			WorkManagerSettingsService workManagerSettingsService) :
			base(navigationService)
		{
			_authenticationService = authenticationService;
			_registrationService = registrationService;
			_toastMessageService = toastMessageService;
			_workManagerSettingsService = workManagerSettingsService;
			LoginCommand = new DelegateCommand(async()=> await LoginAsync(), () => !IsBusy);
			ShowRegisterCommand = new DelegateCommand(Register, () => !IsBusy);
			ContinueWithoutLoginCommand = new DelegateCommand(async ()=>await ContinueWithoutLogin(), () => !IsBusy);
			Username = _workManagerSettingsService.Username;
			Password = _workManagerSettingsService.Password;
			IsRememberCredentialsToggled = _workManagerSettingsService.SaveCredentials;
#if DEBUG
			LoginCommand.Execute();
#endif
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

		private bool _isRememberCredentialsToggled;
		public bool IsRememberCredentialsToggled
		{
			get => _isRememberCredentialsToggled;
			set
			{
				if (_isRememberCredentialsToggled == value) return;
				_isRememberCredentialsToggled = value;
				RaisePropertyChanged();
			}
		}

		private async void Register()
		{
			await NavigationService.NavigateAsync("RegisterPage");
		}

		private async Task LoginAsync()
		{
			BeginProcess();
			if(!IsUsernameRightShowDialog() || !IsPasswordRightShowDialog())
			{
				EndProcess();
				return;
			}
			try
			{
				IUserModel user = await _authenticationService.AuthenticateAsync(Username, Password);
				if (user != null)
					await NavigationService.NavigateAsync("/RootPage/NavigationPage/TaskGroupPage");
				if (IsRememberCredentialsToggled)
				{
					SaveCredentials();
				}
				else
				{
					ClearCredentials();
				}

			}
			catch (UnauthorizedAccessException)
			{
				_toastMessageService.LongAlert(TranslateViewModelsSR.UnauthorizedAccessExceptionMessage);
			}
			finally
			{
				EndProcess();
			}
		}

		private void SaveCredentials()
		{
			_workManagerSettingsService.Username = Username;
			_workManagerSettingsService.Password = Password;
			_workManagerSettingsService.SaveCredentials = IsRememberCredentialsToggled;
		}

		private void ClearCredentials()
		{
			_workManagerSettingsService.Username = string.Empty;
			_workManagerSettingsService.Password = string.Empty;
			_workManagerSettingsService.SaveCredentials = false;
		}

		private bool IsPasswordRightShowDialog()
		{
			if (Password.IsNullOrEmpty())
			{
				_toastMessageService.LongAlert(TranslateViewModelsSR.PasswordIsNullOrEmpty);
				return false;
			}
			return true;
		}

		private bool IsUsernameRightShowDialog()
		{
			if (Username.IsNullOrEmpty())
			{
				_toastMessageService.LongAlert(TranslateViewModelsSR.UserNameIsNullOrEmpty);
				return false;
			}
			return true;
		}

		private async Task ContinueWithoutLogin()
		{
			BeginProcess();
			try
			{
				await _registrationService.RegisterAndAuthenticateUserAsync(_testModel);
			}
			catch (UserAlreadyExistsException)
			{
				if (await _authenticationService.AuthenticateAsync(_testModel.Username, _testModel.Password) != null)
					await NavigationService.NavigateAsync("/RootPage/NavigationPage/TaskGroupPage");
			}
			finally
			{
				EndProcess();
			}
		}
	}
}