using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Pages
{
	public class UserDetailPageViewModel : ViewModelBase
	{
		private readonly ICurrentModelProvider<IUserModel> _currentUserModelProvider;
        private readonly IAuthenticationService _authenticationService;
        private readonly IDialogService _dialogService;

        public UserDetailPageViewModel(INavigationService navigationService, ICurrentModelProvider<IUserModel> currentUserModelProvider, IAuthenticationService authenticationService, IDialogService dialogService) : base(navigationService)
        {
            _currentUserModelProvider = currentUserModelProvider;
            _authenticationService = authenticationService;
            _dialogService = dialogService;
            ChangePasswordCommand = new DelegateCommand(async () => await ChangePasswordAsync());
        }

        public DelegateCommand ChangePasswordCommand { get; }

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

        protected override async Task InitializeAsyncInt()
        {
            await base.InitializeAsyncInt();
            Username = _currentUserModelProvider.GetModel().Username;
            FirstName = _currentUserModelProvider.GetModel().FirstName;
            Surname = _currentUserModelProvider.GetModel().Surname;
        }

        private async Task ChangePasswordAsync()
        {
            BeginProcess();
            await _dialogService.ShowDialogAsync("ChangePasswordDialog");
            EndProcess();
        }
    }
}