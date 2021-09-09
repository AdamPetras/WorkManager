using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.Enums;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddCompanyDialogViewModel : DialogViewModelBase
	{
		private readonly ICompanyFacade _companyFacade;
		private readonly IUserFacade _userFacade;
		private readonly ICurrentModelProvider<IUserModel> _currentUserProvider;
		private readonly IEventAggregator _eventAggregator;
		private readonly IToastMessageService _toastMessageService;
		private EWorkType _selectedWorkType;

		public AddCompanyDialogViewModel(INavigationService navigationService, ICompanyFacade companyFacade, IUserFacade userFacade,
			ICurrentModelProvider<IUserModel> currentUserProvider, IEventAggregator eventAggregator, IToastMessageService toastMessageService) : base(navigationService)
		{
			_companyFacade = companyFacade;
			_userFacade = userFacade;
			_currentUserProvider = currentUserProvider;
			_eventAggregator = eventAggregator;
			_toastMessageService = toastMessageService;
			CancelCommand = new DelegateCommand(Cancel);
			ConfirmCommand = new DelegateCommand(Confirm);
		}

		public DelegateCommand CancelCommand { get; }
		public DelegateCommand ConfirmCommand { get; }

		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				if (_name == value) return;
				_name = value;
				RaisePropertyChanged();
			}
		}

		private void Cancel()
		{
			OnRequestClose(null);
		}

		private void Confirm()
		{
			if (_companyFacade.GetAll().Any(s => s.Name == Name))
			{
				_toastMessageService.LongAlert(AddCompanyDialogViewModelSR.NameAlreadyExistsFormat(Name));
				Cancel();
				return;
			}
			ICompanyModel model = new CompanyModel(Guid.NewGuid(), Name, _currentUserProvider.GetModel());
			_companyFacade.Add(model);
			OnRequestClose(new DialogParameters(){{"DialogEvent",new AddAfterDialogCloseDialogEvent<ICompanyModel>(model) }});
		}
	}
}