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
		private readonly IToastMessageService _toastMessageService;
		private readonly WorkManagerSettingsService _workManagerSettingsService;


		public LoginPageViewModel(INavigationService navigationService, IAuthenticationService authenticationService,
            IToastMessageService toastMessageService,
            WorkManagerSettingsService workManagerSettingsService, ViewModelTaskExecute viewModelTaskExecute) :
			base(navigationService, viewModelTaskExecute)
		{
			_authenticationService = authenticationService;
			_toastMessageService = toastMessageService;
			_workManagerSettingsService = workManagerSettingsService;
			Username = _workManagerSettingsService.Username;
			Password = _workManagerSettingsService.Password;
			IsRememberCredentialsToggled = _workManagerSettingsService.SaveCredentials;
            InitDialogCommands();
#if DEBUG
			//LoginCommand.Execute();
#endif
		}

		public DelegateCommand ShowRegisterCommand { get; private set; }
        public DelegateCommand LoginCommand { get; private set; }

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

        protected override void DestroyInt()
        {
            base.DestroyInt();
            DialogThrownEvent -= ShowRegisterCommand.RaiseCanExecuteChanged;
            DialogThrownEvent -= LoginCommand.RaiseCanExecuteChanged;
		}

        private void InitDialogCommands()
        {
			ShowRegisterCommand = new DelegateCommand(Register, () => !IsBusy);
            DialogThrownEvent += ShowRegisterCommand.RaiseCanExecuteChanged;
			LoginCommand = new DelegateCommand(async()=> await LoginAsync(), () => !IsBusy);
            DialogThrownEvent += LoginCommand.RaiseCanExecuteChanged;
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
				IUserModel user = await ViewModelTaskExecute.ExecuteTaskWithQueue(Username, Password, _authenticationService.AuthenticateAsync);
				if (user != null)
                {
                    await NavigationService.NavigateAsync("/RootPage/NavigationPage/TaskGroupPage");
                }
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
	}
}