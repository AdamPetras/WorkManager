using Prism.Mvvm;
using Prism.Navigation;

namespace WorkManager.ViewModels.BaseClasses
{
	public abstract class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
	{
		protected INavigationService NavigationService { get; private set; }

		private bool _isBusy;
		public bool IsBusy
		{
			get => _isBusy;
			protected set
			{
				if (_isBusy == value) return;
				_isBusy = value;
				RaisePropertyChanged();
			}
		}

		protected ViewModelBase(INavigationService navigationService)
		{
			IsBusy = true;
			NavigationService = navigationService;
		}

		public void Initialize(INavigationParameters parameters)
		{
			InitializeInt();
		}

		protected virtual void InitializeInt()
		{
		}

		public void OnNavigatedFrom(INavigationParameters parameters)
		{
			IsBusy = true;
			OnNavigatedFromInt(parameters);
		}

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			OnNavigatedToInt(parameters);
			IsBusy = false;
		}

		protected virtual void OnNavigatedFromInt(INavigationParameters parameters)
		{
		}

		protected virtual void OnNavigatedToInt(INavigationParameters parameters)
		{
		}

		public void Destroy()
		{
			DestroyInt();
		}

		protected virtual void DestroyInt()
		{

		}
	}
}
