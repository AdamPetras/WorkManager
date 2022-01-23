using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using WorkManager.BL.Exceptions;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Dialogs
{
    public class ChangePasswordDialogViewModel : ConfirmDialogViewModelBase
    {
        private readonly ICurrentModelProvider<IUserModel> _currentUserModelProvider;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserFacade _userFacade;
        private readonly IToastMessageService _toastMessageService;

        public ChangePasswordDialogViewModel(INavigationService navigationService, ICurrentModelProvider<IUserModel> currentUserModelProvider, IAuthenticationService authenticationService, 
            IUserFacade userFacade, IToastMessageService toastMessageService) : base(navigationService)
        {
            _currentUserModelProvider = currentUserModelProvider;
            _authenticationService = authenticationService;
            _userFacade = userFacade;
            _toastMessageService = toastMessageService;
        }

        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                if (_oldPassword == value) return;
                _oldPassword = value;
                RaisePropertyChanged();
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (_newPassword == value) return;
                _newPassword = value;
                RaisePropertyChanged();
            }
        }

        private string _repeatNewPassword;
        public string RepeatNewPassword
        {
            get => _repeatNewPassword;
            set
            {
                if (_repeatNewPassword == value) return;
                _repeatNewPassword = value;
                RaisePropertyChanged();
            }
        }

        protected override async Task ConfirmAsyncInt()
        {
            if (string.IsNullOrWhiteSpace(OldPassword))
            {
                _toastMessageService.LongAlert(TranslateViewModelsSR.PasswordIsNullOrEmpty);
                return;
            }

            if (NewPassword != RepeatNewPassword)
            {
                _toastMessageService.LongAlert(TranslateViewModelsSR.NewPasswordsAreNotSame);
                return;
            }

            try
            {
                if (_authenticationService.HasPasswordCorrectStructure(NewPassword))
                {
                    BeginProcess();
                    if (await _authenticationService.PasswordMatchesHashedPasswordAsync(OldPassword,
                            _currentUserModelProvider.GetModel().Password))
                    {
                        IUserModel cpyModel = new UserModel(_currentUserModelProvider.GetModel());
                        cpyModel.Password = await _authenticationService.GetHashedPasswordAsync(NewPassword);
                        await _userFacade.UpdateAsync(cpyModel);
                        CancelInt();
                    }
                }
            }
            catch (PasswordStructureException e)
            {
                _toastMessageService.LongAlert(e.Message);
            }
            finally
            {
                EndProcess();
            }
           
        }
    }
}