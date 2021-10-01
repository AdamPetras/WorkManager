using Prism.Navigation;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Pages
{
	public class UserDetailPageViewModel : ViewModelBase
	{
		private readonly ICurrentModelProvider<IUserModel> _currentUserModelProvider;

		public UserDetailPageViewModel(INavigationService navigationService, ICurrentModelProvider<IUserModel> currentUserModelProvider) : base(navigationService)
		{
			_currentUserModelProvider = currentUserModelProvider;
			User = _currentUserModelProvider.GetModel();
		}

		private IUserModel _user;
		public IUserModel User
		{
			get => _user;
			set
			{
				if (_user == value) return;
				_user = value;
				RaisePropertyChanged();
			}
		}
	}
}