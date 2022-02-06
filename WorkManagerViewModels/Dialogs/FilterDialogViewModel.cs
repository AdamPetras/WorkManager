using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.NavigationParams;
using WorkManager.DAL.Enums;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Dialogs
{
	public class FilterDialogViewModel:ConfirmDialogViewModelBase
	{
		public FilterDialogViewModel(INavigationService navigationService, ViewModelTaskExecute viewModelTaskExecute) : base(navigationService, viewModelTaskExecute)
		{
		}

		
        private string _dialogTitle;
        public string DialogTitle
        {
            get => _dialogTitle;
            set
            {
                if (_dialogTitle == value) return;
                _dialogTitle = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _dateFrom;
        public DateTime DateFrom
        {
            get => _dateFrom;
            set
            {
                if (_dateFrom == value) return;
                _dateFrom = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _dateTo;
        public DateTime DateTo
        {
            get => _dateTo;
            set
            {
                if (_dateTo == value) return;
                _dateTo = value;
                RaisePropertyChanged();
            }
        }

#pragma warning disable CS1998
        protected override async Task ConfirmAsyncInt()
#pragma warning restore CS1998
        {
            OnRequestClose(new FilterNavigationParameters(DialogTitle,DateFrom,DateTo));
        }

        protected override void OnDialogOpenedInt(IDialogParameters parameters)
		{
			base.OnDialogOpenedInt(parameters);
            FilterNavigationParameters navParams = new FilterNavigationParameters(parameters);
            DialogTitle = navParams.Title;
            DateFrom = navParams.DateFrom;
            DateTo = navParams.DateTo;

        }
	}
}