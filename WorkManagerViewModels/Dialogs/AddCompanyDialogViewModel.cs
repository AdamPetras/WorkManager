using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Core;
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
		private readonly ICurrentModelProvider<IUserModel> _currentUserProvider;
		private readonly IToastMessageService _toastMessageService;

		public AddCompanyDialogViewModel(INavigationService navigationService, ICompanyFacade companyFacade,
			ICurrentModelProvider<IUserModel> currentUserProvider, IToastMessageService toastMessageService) : base(navigationService)
		{
			_companyFacade = companyFacade;
			_currentUserProvider = currentUserProvider;
			_toastMessageService = toastMessageService;
			CancelCommand = new DelegateCommand(Cancel);
			ConfirmCommand = new DelegateCommand(async() => await Confirm());
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

		private async Task Confirm()
		{
			BeginProcess();
			if (await _companyFacade.ExistsAsync(Name))
			{
				_toastMessageService.LongAlert(TranslateViewModelsSR.CompanyNameAlreadyExists.Format(Name));
				Cancel();
				return;
			}
			ICompanyModel model = new CompanyModel(Guid.NewGuid(), Name,0, _currentUserProvider.GetModel().Id);
			await _companyFacade.AddAsync(model);
			OnRequestClose(new DialogParameters(){{"DialogEvent",new AddAfterDialogCloseDialogEvent<ICompanyModel>(model) }});
			EndProcess();
		}
	}
}