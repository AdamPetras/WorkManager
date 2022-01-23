using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;

namespace WorkManager.ViewModels.BaseClasses
{
	public abstract class ConfirmDialogViewModelBase : DialogViewModelBase
	{
		protected ConfirmDialogViewModelBase(INavigationService navigationService) : base(navigationService)
        {
            CancelCommand = new DelegateCommand(CancelInt);
            ConfirmCommand = new DelegateCommand(async() => await ConfirmAsyncInt());
        }


        public DelegateCommand CancelCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        protected virtual void CancelInt()
        {
            OnRequestClose(null);
        }
        protected abstract Task ConfirmAsyncInt();
    }
}