using Prism.Mvvm;
using Prism.Navigation;

namespace WorkManager.ViewModels.BaseClasses
{
	public abstract class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
	{
		protected INavigationService NavigationService { get; private set; }

		protected ViewModelBase(INavigationService navigationService)
		{
			NavigationService = navigationService;
		}

		public virtual void Initialize(INavigationParameters parameters)
		{
			InitializeInt();
		}

		protected virtual void InitializeInt()
		{
		}

		public void OnNavigatedFrom(INavigationParameters parameters)
		{
			OnNavigatedFromInt(parameters);
		}

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			OnNavigatedToInt(parameters);
		}

		protected virtual void OnNavigatedFromInt(INavigationParameters parameters)
		{
		}

		protected virtual void OnNavigatedToInt(INavigationParameters parameters)
		{
		}

		public virtual void Destroy()
		{

		}
	}
}
