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
			NavigationService = navigationService;
		}

		public event DialogThrownDelegate DialogThrownEvent;

		/// <summary>
		/// Pokud program vykonává složitější operaci nastavíme na IsBusy == true následně po vykonání IsBusy==false zobrazí nadefinovaný activity indicator pouze async popř pokud operace běží na jiném než hlavnín vlákně
		/// </summary>
		public bool IsBusy => RunningOperation != 0;

		private ushort _runningOperation;
		private ushort RunningOperation
		{
			get => _runningOperation;
			set
			{
				_runningOperation = value;
				RaisePropertyChanged(nameof(IsBusy));
			}
		}

		protected void BeginProcess()
		{
			RunningOperation++;
		}

		protected void EndProcess()
		{
			if(RunningOperation != 0)
				RunningOperation--;
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
			BeginProcess();
			OnNavigatedFromInt(parameters);
			EndProcess();
		}

		public void OnNavigatedTo(INavigationParameters parameters)
		{
			BeginProcess();
			OnNavigatedToInt(parameters);
			EndProcess();
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
