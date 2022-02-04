using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.NavigationParams;
using WorkManager.BL.Services;
using WorkManager.Extensions;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class CompanyPageViewModel : CollectionViewModelBase
	{
		private readonly ICurrentModelProvider<IUserModel> _currentUserProvider;
		private readonly ICurrentModelProviderManager<ICompanyModel> _companyModelProviderManager;
		private readonly IDialogService _dialogService;
		private readonly ICompanyFacade _companyFacade;
		private readonly DialogEventService _dialogEventService;
		private readonly IPageDialogService _pageDialogService;
        private readonly RecordTotalCalculatorService _recordTotalCalculatorService;
        private readonly IWorkRecordFacade _workRecordFacade;

        public CompanyPageViewModel(INavigationService navigationService, ICurrentModelProvider<IUserModel> currentUserProvider, ICurrentModelProviderManager<ICompanyModel> companyModelProviderManager,
			IDialogService dialogService, ICompanyFacade companyFacade, DialogEventService dialogEventService, IPageDialogService pageDialogService,RecordTotalCalculatorService recordTotalCalculatorService,
            IWorkRecordFacade workRecordFacade) : base(navigationService)
		{
			_currentUserProvider = currentUserProvider;
			_companyModelProviderManager = companyModelProviderManager;
			_dialogService = dialogService;
			_companyFacade = companyFacade;
			_dialogEventService = dialogEventService;
			_pageDialogService = pageDialogService;
            _recordTotalCalculatorService = recordTotalCalculatorService;
            _workRecordFacade = workRecordFacade;
            ShowWorkPageCommand = new DelegateCommand<ICompanyModel>(async (s) => await ShowWorkPageAsync(s));
			EditCommand = new DelegateCommand<ICompanyModel>(Edit);
			DeleteCompanyCommand = new DelegateCommand<ICompanyModel>(async (s) => await DeleteCompanyAsync(s));
            RefreshCommand = new DelegateCommand(async () => {
                BeginRefresh();
                await RefreshAsync();
                EndRefresh();
            });
			InitDialogCommands();
		}

        public DelegateCommand RefreshCommand { get; }
		public DelegateCommand ClearCompaniesCommand { get; private set; }
		public DelegateCommand ShowAddCompanyDialogCommand { get; private set; }
		public DelegateCommand<ICompanyModel> ShowWorkPageCommand { get; }
		public DelegateCommand<ICompanyModel> EditCommand { get; }
		public DelegateCommand<ICompanyModel> DeleteCompanyCommand { get; }

		private ObservableCollection<ICompanyModel> _companies;
		public ObservableCollection<ICompanyModel> Companies
		{
			get => _companies;
			private set
			{
				if (_companies == value)
					return;
				_companies = value;
				RaisePropertyChanged();
			}
		}

        protected override async Task InitializeAsyncInt()
        {
            await base.InitializeAsyncInt();
            await RefreshAsync();
        }

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= ShowAddCompanyDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ClearCompaniesCommand.RaiseCanExecuteChanged;
		}

		private void InitDialogCommands()
		{
			ShowAddCompanyDialogCommand = new DelegateCommand(async () => await ShowAddCompanyDialogAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ShowAddCompanyDialogCommand.RaiseCanExecuteChanged;
			ClearCompaniesCommand = new DelegateCommand(async () => await ClearCompaniesAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ClearCompaniesCommand.RaiseCanExecuteChanged;
		}

		private async Task ShowWorkPageAsync(ICompanyModel obj)
		{
			BeginProcess();
			_companyModelProviderManager.SetItem(obj);
			await NavigationService.NavigateAsync("WorkRecordPage");
			EndProcess();
		}

		private async Task ClearCompaniesAsync()
		{
			BeginProcess();
			IsDialogThrown = true;
			if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
				TranslateViewModelsSR.CompanyClearDialogMessage, TranslateViewModelsSR.DialogYes,
				TranslateViewModelsSR.DialogNo))
			{
				await _companyFacade.ClearAsync();
				Companies.Clear();
			}
			IsDialogThrown = false;
			EndProcess();
		}

		private async Task DeleteCompanyAsync(ICompanyModel companyModel)
		{
			BeginProcess();
			await _companyFacade.RemoveAsync(companyModel.Id);
			Companies.Remove(companyModel);
			EndProcess();
		}

		private async Task ShowAddCompanyDialogAsync()
		{
			IsDialogThrown = true;
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddCompanyDialog")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, Companies);
			IsDialogThrown = false;
		}

        private async Task RefreshAsync()
        {
			BeginProcess();
			Companies = new ObservableCollection<ICompanyModel>(await _companyFacade.GetCompaniesByUserIdAsync(_currentUserProvider.GetModel().Id));
			EndProcess();
		}

		private void Edit(ICompanyModel companyModel)
		{
            BeginProcess();
            NavigationService.NavigateAsync("CompanyDetailPage", new CompanyNavigationParameters(new CompanyModel(companyModel)));
            EndProcess();
		}
	}
}