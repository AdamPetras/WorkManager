using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace WorkManager.ViewModels.BaseClasses
{
	public delegate void DialogThrownDelegate();
	public abstract class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
	{
		protected ViewModelBase(INavigationService navigationService)
		{
			IsBusy = true;
			NavigationService = navigationService;
		}

		public event DialogThrownDelegate DialogThrownEvent;

		private bool _isBusy;
		/// <summary>
		/// Pokud program vykonává složitější operaci nastavíme na IsBusy == true následně po vykonání IsBusy==false zobrazí nadefinovaný activity indicator pouze async popř pokud operace běží na jiném než hlavnín vlákně
		/// </summary>
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

		private bool _isDialogThrown;
		/// <summary>
		/// Řeší problém s více spuštěnými dialogy neumožní spustit více dialogů najednou a zároveň disabluje tlačítka toolbaru
		/// </summary>
		public bool IsDialogThrown
		{
			get => _isDialogThrown;
			set
			{
				if (_isDialogThrown == value) return;
				_isDialogThrown = value;
				RaisePropertyChanged();
				OnDialogThrownEvent();
			}
		}
		protected INavigationService NavigationService { get; private set; }

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

		private void OnDialogThrownEvent()
		{
			DialogThrownEvent?.Invoke();
		}
	}
}
