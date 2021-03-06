using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using WorkManager.BL.Exceptions;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Core;
using WorkManager.Models;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class RegisterPageViewModel : ViewModelBase
	{
		private readonly IRegistrationService _registrationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IToastMessageService _toastMessageService;

		public RegisterPageViewModel(INavigationService navigationService, IRegistrationService registrationService,
            IAuthenticationService authenticationService, IToastMessageService toastMessageService,
            ViewModelTaskExecute viewModelTaskExecute) : base(navigationService, viewModelTaskExecute)
		{
			_registrationService = registrationService;
            _authenticationService = authenticationService;
            _toastMessageService = toastMessageService;
			RegisterCommand = new DelegateCommand(Register);
		}

		public DelegateCommand RegisterCommand { get; }

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

		private string _repeatPassword;
		public string RepeatPassword
		{
			get => _repeatPassword;
			set
			{
				if (_repeatPassword == value) return;
				_repeatPassword = value;
				RaisePropertyChanged();
			}
		}

		private string _firstName;
		public string FirstName
		{
			get => _firstName;
			set
			{
				if (_firstName == value) return;
				_firstName = value;
				RaisePropertyChanged();
			}
		}

		private string _surname;
		public string Surname
		{
			get => _surname;
			set
			{
				if (_surname == value) return;
				_surname = value;
				RaisePropertyChanged();
			}
		}

		private async void Register()
		{
			BeginProcess();
            if (RepeatPassword != Password)
            {
                _toastMessageService.LongAlert(TranslateViewModelsSR.PasswordsAreNotSame);
            }
            
            try
            {
                if (_authenticationService.HasPasswordCorrectStructure(Password))
                {
                    if (await ViewModelTaskExecute.ExecuteTaskWithQueue(new UserModel(Guid.NewGuid(), FirstName, Surname, Username, Password),
                            _registrationService.RegisterAndAuthenticateUserAsync))
                        await NavigationService.NavigateAsync("/RootPage/NavigationPage/TaskGroupPage");
                }
            }
            catch (PasswordStructureException e)
            {
                _toastMessageService.LongAlert(e.Message);
			}
			catch (UserAlreadyExistsException)
            {
                _toastMessageService.LongAlert(TranslateViewModelsSR.UserAlreadyExists.Format(Username));
            }
            catch (UserNotExistsException e)
            {
                Crashes.TrackError(e);
            }
            finally
            {
                EndProcess();
            }
        }
	}
}