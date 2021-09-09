using System;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace WorkManager.ViewModels.BaseClasses
{
	public abstract class DialogViewModelBase : ViewModelBase, IDialogAware
	{
		protected DialogViewModelBase(INavigationService navigationService) : base(navigationService)
		{
		}

		public bool CanCloseDialog() => true;

		public void OnDialogClosed()
		{
		}

		public void OnDialogOpened(IDialogParameters parameters)
		{
			OnDialogOpenedInt(parameters);
		}

		protected virtual void OnDialogOpenedInt(IDialogParameters parameters)
		{
		}

		public event Action<IDialogParameters> RequestClose;

		protected void OnRequestClose(IDialogParameters obj)
		{
			RequestClose?.Invoke(obj);
		}
	}
}