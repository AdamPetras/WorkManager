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
		private readonly DialogEventService _dialogEventService;
		private readonly IPageDialogService _pageDialogService;

		public CompanyPageViewModel(INavigationService navigationService, ICurrentModelProvider<IUserModel> currentUserProvider, ICurrentModelProviderManager<ICompanyModel> companyModelProviderManager,
			IDialogService dialogService, ICompanyFacade companyFacade, DialogEventService dialogEventService, IPageDialogService pageDialogService) : base(navigationService)
		{
			_currentUserProvider = currentUserProvider;
			_companyModelProviderManager = companyModelProviderManager;
			_dialogService = dialogService;
			_companyFacade = companyFacade;
			_dialogEventService = dialogEventService;
			_pageDialogService = pageDialogService;
			ShowWorkPageCommand = new DelegateCommand<ICompanyModel>(async(s) => await ShowWorkPageAsync(s));
			SelectCompanyCommand = new DelegateCommand<ICompanyModel>(SelectCompany);
			EditCommand = new DelegateCommand(Edit, () => SelectedCompany != null);
			InitDialogCommands();
		}

		public DelegateCommand<ICompanyModel> SelectCompanyCommand { get; }
		public DelegateCommand ClearWholeOrDeleteSingleCompanyCommand { get; private set; }
		public DelegateCommand ShowAddCompanyDialogCommand { get; private set; }
		public DelegateCommand<ICompanyModel> ShowWorkPageCommand { get; }
		public DelegateCommand EditCommand { get; }


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

		private ICompanyModel _selectedCompany;
		public ICompanyModel SelectedCompany
		{
			get => _selectedCompany;
			set
			{
				if (_selectedCompany == value) return;
				_selectedCompany = value;
				RaisePropertyChanged();
				EditCommand.RaiseCanExecuteChanged();
			}
		}

		protected override void InitializeInt()
		{
			base.InitializeInt();
			Companies = new ObservableCollection<ICompanyModel>(_companyFacade.GetCompaniesByUserId(_currentUserProvider.GetModel().Id));
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			if (Companies == null || Companies.Count == 0)
				Companies = new ObservableCollection<ICompanyModel>(_companyFacade.GetCompaniesByUserId(_currentUserProvider.GetModel().Id));
		}

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= ShowAddCompanyDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ClearWholeOrDeleteSingleCompanyCommand.RaiseCanExecuteChanged;
		}

		private void InitDialogCommands()
		{
			ShowAddCompanyDialogCommand = new DelegateCommand(async () => await ShowAddCompanyDialogAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ShowAddCompanyDialogCommand.RaiseCanExecuteChanged;
			ClearWholeOrDeleteSingleCompanyCommand = new DelegateCommand(async () => await ClearWholeOrDeleteSingleCompanyAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ClearWholeOrDeleteSingleCompanyCommand.RaiseCanExecuteChanged;
		}

		private async Task ShowWorkPageAsync(ICompanyModel obj)
		{
			BeginProcess();
			_companyModelProviderManager.SetItem(obj);
			await NavigationService.NavigateAsync("WorkRecordPage");
			EndProcess();
		}
		private void SelectCompany(ICompanyModel companyModel)
		{
			SelectedCompany = companyModel;
		}

		private async Task ClearWholeOrDeleteSingleCompanyAsync()
		{
			BeginProcess();
			if (_selectedCompany != null)
			{
				if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
					TranslateViewModelsSR.SelectedCompanyDeleteDialogMessageFormat(_selectedCompany.Name), TranslateViewModelsSR.DialogYes,
					TranslateViewModelsSR.DialogNo))
				{
					await _companyFacade.RemoveAsync(_selectedCompany.Id);
					Companies.Remove(_selectedCompany);
					_selectedCompany = null;
				}
			}
			else
			{
				IsDialogThrown = true;
				if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
					TranslateViewModelsSR.CompanyClearDialogMessage, TranslateViewModelsSR.DialogYes,
					TranslateViewModelsSR.DialogNo))
				{
					await _companyFacade.ClearAsync();
					Companies.Clear();
				}
				IsDialogThrown = false;
			}
			EndProcess();
		}

		private async Task ShowAddCompanyDialogAsync()
		{
			IsDialogThrown = true;
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddCompanyDialogView")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, Companies);
			IsDialogThrown = false;
		}

		private void Edit()
		{

		}
	}
}