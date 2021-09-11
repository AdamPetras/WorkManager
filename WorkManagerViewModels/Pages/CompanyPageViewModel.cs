using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Services;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class CompanyPageViewModel : ViewModelBase
	{
		private readonly ICurrentModelProvider<IUserModel> _currentUserProvider;
		private readonly ICurrentModelProviderManager<ICompanyModel> _companyModelProviderManager;
		private readonly IDialogService _dialogService;
		private readonly ICompanyFacade _companyFacade;
		private readonly IEventAggregator _eventAggregator;
		private readonly DialogEventService _dialogEventService;
		private readonly IPageDialogService _pageDialogService;
		private ICompanyModel _selectedCompany;

		public CompanyPageViewModel(INavigationService navigationService, ICurrentModelProvider<IUserModel> currentUserProvider, ICurrentModelProviderManager<ICompanyModel> companyModelProviderManager,
			IDialogService dialogService, ICompanyFacade companyFacade, IEventAggregator eventAggregator, DialogEventService dialogEventService, IPageDialogService pageDialogService) : base(navigationService)
		{
			_currentUserProvider = currentUserProvider;
			_companyModelProviderManager = companyModelProviderManager;
			_dialogService = dialogService;
			_companyFacade = companyFacade;
			_eventAggregator = eventAggregator;
			_dialogEventService = dialogEventService;
			_pageDialogService = pageDialogService;
			ShowAddCompanyDialogCommand = new DelegateCommand(async()=>await ShowAddCompanyDialogAsync());
			ShowWorkPageCommand = new DelegateCommand<ICompanyModel>(async(s) => await ShowWorkPageAsync(s));
			SelectCompanyCommand = new DelegateCommand<ICompanyModel>(SelectCompany);
			ClearWholeOrDeleteSingleCompanyCommand = new DelegateCommand(async () => ClearWholeOrDeleteSingleCompanyAsync());
		}

		public DelegateCommand<ICompanyModel> SelectCompanyCommand { get; }
		public DelegateCommand ClearWholeOrDeleteSingleCompanyCommand { get; }
		public DelegateCommand ShowAddCompanyDialogCommand { get; }
		public DelegateCommand<ICompanyModel> ShowWorkPageCommand { get; }

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



		protected override void InitializeInt()
		{
			base.InitializeInt();
			Companies = new ObservableCollection<ICompanyModel>(_companyFacade.GetCompaniesByUserId(_currentUserProvider.GetModel().Id));
		}

		private async Task ShowAddCompanyDialogAsync()
		{
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddCompanyDialogView")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, Companies);
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			if (Companies.Count == 0)
				Companies = new ObservableCollection<ICompanyModel>(_companyFacade.GetCompaniesByUserId(_currentUserProvider.GetModel().Id));
		}

		private async Task ShowWorkPageAsync(ICompanyModel obj)
		{
			_companyModelProviderManager.SetItem(obj);
			await NavigationService.NavigateAsync("WorkRecordPage");
		}
		private void SelectCompany(ICompanyModel companyModel)
		{
			_selectedCompany = companyModel;
		}

		private async Task ClearWholeOrDeleteSingleCompanyAsync()
		{
			if (_selectedCompany != null)
			{
				await _companyFacade.RemoveAsync(_selectedCompany.Id);
				Companies.Remove(_selectedCompany);
				_selectedCompany = null;
			}
			else
			{
				if (await _pageDialogService.DisplayAlertAsync(CompanyPageViewModelSR.ClearDialogTitle,
					CompanyPageViewModelSR.ClearDialogMessage, CompanyPageViewModelSR.DialogYes,
					CompanyPageViewModelSR.DialogNo))
				{
					_companyFacade.Clear();
					Companies.Clear();
				}
			}
		}
	}
}