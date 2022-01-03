﻿using System;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.NavigationParams;
using WorkManager.DAL.Enums;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Dialogs
{
	public class FilterDialogViewModel:DialogViewModelBase
	{
		public FilterDialogViewModel(INavigationService navigationService) : base(navigationService)
		{
			ConfirmCommand = new DelegateCommand(Confirm);
			CancelCommand = new DelegateCommand(Cancel);
		}

		public DelegateCommand ConfirmCommand { get; }
		public DelegateCommand CancelCommand { get; }
		
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
		
        protected override void OnDialogOpenedInt(IDialogParameters parameters)
		{
			base.OnDialogOpenedInt(parameters);
            FilterNavigationParameters navParams = new FilterNavigationParameters(parameters);
            DialogTitle = navParams.Title;
            DateFrom = navParams.DateFrom;
            DateTo = navParams.DateTo;

        }

		private void Confirm()
		{
			OnRequestClose(new FilterNavigationParameters(DialogTitle,DateFrom,DateTo));
		}

		private void Cancel()
		{
			OnRequestClose(null);
		}
	}
}