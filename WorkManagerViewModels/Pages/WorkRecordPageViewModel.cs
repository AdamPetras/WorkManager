using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.NavigationParams;
using WorkManager.BL.Services;
using WorkManager.Core;
using WorkManager.DAL.Enums;
using WorkManager.Extensions;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class WorkRecordPageViewModel : CollectionViewModelBase
	{
		private readonly ICurrentModelProvider<ICompanyModel> _companyModelProvider;
		private readonly IRecordTotalCalculatorService _recordTotalCalculatorService;
		private readonly IWorkRecordFacade _workFacade;
		private readonly IDialogService _dialogService;
		private readonly DialogEventService _dialogEventService;
		private readonly IPageDialogService _pageDialogService;

        private DateTime _filterDateFrom;
		private DateTime _filterDateTo;


		public WorkRecordPageViewModel(INavigationService navigationService, ICurrentModelProvider<ICompanyModel> companyModelProvider, IRecordTotalCalculatorService recordTotalCalculatorService,
			IWorkRecordFacade workFacade, IDialogService dialogService, DialogEventService dialogEventService, IPageDialogService pageDialogService) : base(navigationService)
		{
			_companyModelProvider = companyModelProvider;
            _recordTotalCalculatorService = recordTotalCalculatorService;
			_workFacade = workFacade;
			_dialogService = dialogService;
			_dialogEventService = dialogEventService;
			_pageDialogService = pageDialogService;
            _filterDateFrom = DateTime.Today.Subtract(TimeSpan.FromDays(31));
            _filterDateTo = DateTime.Today;
			EditCommand = new DelegateCommand<IWorkRecordModelBase>(async (s) => await EditAsync(s));
            DeleteRecordCommand = new DelegateCommand<IWorkRecordModelBase>(async (s) => await DeleteRecordAsync(s));
			RefreshCommand = new DelegateCommand(async () => {
				BeginRefresh();
				await RefreshAsync();
				EndRefresh();
			});
			InitDialogCommands();
		}

        public DelegateCommand RefreshCommand { get; }
		public DelegateCommand ClearRecordsCommand { get; private set; }
		public DelegateCommand<IWorkRecordModelBase> DeleteRecordCommand { get; private set; }
		public DelegateCommand ShowFilterDialogCommand { get; private set; }
		public DelegateCommand ShowStatisticsCommand { get; private set; }
		public DelegateCommand<IWorkRecordModelBase> EditCommand { get; }
		public DelegateCommand ShowAddDialogCommand { get; private set; }

		private ObservableCollection<IWorkRecordModelBase> _records;

		public ObservableCollection<IWorkRecordModelBase> Records
		{
			get => _records;
			set
			{
				if (_records == value) return;
				_records = value;
				RaisePropertyChanged();
			}
		}

        private double _totalPriceThisMonth;
        public double TotalPriceThisMonth
        {
            get => _totalPriceThisMonth;
            set
            {
                if (_totalPriceThisMonth == value) return;
                _totalPriceThisMonth = value;
                RaisePropertyChanged();
            }
        }

        private double _totalPriceThisYear;
        public double TotalPriceThisYear
        {
            get => _totalPriceThisYear;
            set
            {
                if (_totalPriceThisYear == value) return;
                _totalPriceThisYear = value;
                RaisePropertyChanged();
            }
        }

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= ShowAddDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ShowStatisticsCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ShowFilterDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ClearRecordsCommand.RaiseCanExecuteChanged;
		}

        protected override async Task InitializeAsyncInt()
        {
            await base.InitializeAsyncInt();
            await RefreshAsync();
			UpdateTotalPrices();
		}

        protected override async Task OnNavigatedToAsyncInt(INavigationParameters parameters)
		{
			await base.OnNavigatedToAsyncInt(parameters);
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, Records);
			UpdateTotalPrices();
		}

		private void UpdateTotalPrices()
        {
            TotalPriceThisMonth = _recordTotalCalculatorService.CalculateThisMonth(Records);
            TotalPriceThisYear = _recordTotalCalculatorService.CalculateThisYear(Records);
		}

		private void InitDialogCommands()
		{
			ShowAddDialogCommand = new DelegateCommand(async () => await ShowAddDialogAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ShowAddDialogCommand.RaiseCanExecuteChanged;
			ShowStatisticsCommand = new DelegateCommand(ShowStatistics, () => !IsDialogThrown);
			DialogThrownEvent += ShowStatisticsCommand.RaiseCanExecuteChanged;
			ShowFilterDialogCommand = new DelegateCommand(async () => await ShowFilterDialog(), () => !IsDialogThrown);
			DialogThrownEvent += ShowFilterDialogCommand.RaiseCanExecuteChanged;
			ClearRecordsCommand = new DelegateCommand(async () => await ClearRecordsAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ClearRecordsCommand.RaiseCanExecuteChanged;
		}

		private async Task ShowAddDialogAsync()
		{
			IsDialogThrown = true;
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddWorkRecordDialogView")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, Records);
			UpdateTotalPrices();
			Records = new System.Collections.ObjectModel.ObservableCollection<IWorkRecordModelBase>(Records.OrderByDescending(s => s.ActualDateTime));
			IsDialogThrown = false;
		}

		private void ShowStatistics()
		{
			IsDialogThrown = true;
			IsDialogThrown = false;
		}

		private async Task ShowFilterDialog()
		{
			IsDialogThrown = true;
            IDialogParameters parameters = (await _dialogService.ShowDialogAsync("FilterDialogView", new FilterNavigationParameters(TranslateViewModelsSR.FilterTitle, _filterDateFrom, _filterDateTo))).Parameters;
            if (parameters.Any()) //parameters je typ enumerable tudíž rychlejší přístup je využít Any() namísto Count() == 0
            {
				FilterNavigationParameters filterParams = (FilterNavigationParameters) parameters;
                if (_filterDateFrom.Date != filterParams.DateFrom || _filterDateTo != filterParams.DateTo)
                {
                    _filterDateFrom = filterParams.DateFrom;
                    _filterDateTo = filterParams.DateTo;
                    await RefreshAsync();
                }
            }
			IsDialogThrown = false;
		}

		private async Task ClearRecordsAsync()
		{
			BeginProcess();
			IsDialogThrown = true;
			if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
				TranslateViewModelsSR.WorkRecordClearDialogMessage, TranslateViewModelsSR.DialogYes,
				TranslateViewModelsSR.DialogNo))
			{
				await _workFacade.ClearAsync();
				Records?.Clear();
                _companyModelProvider.GetModel().WorkRecordsCount = 0;
			}
            IsDialogThrown = false;
            UpdateTotalPrices();
            EndProcess();
		}

        private async Task RefreshAsync()
        {
			BeginProcess();
            Records = new ObservableCollection<IWorkRecordModelBase>(await _workFacade.GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(_companyModelProvider.GetModel().Id,_filterDateFrom,_filterDateTo).ToListAsync());
			EndProcess();
        }

		private async Task DeleteRecordAsync(IWorkRecordModelBase workRecordModelBase)
        {
			BeginProcess();
            if (workRecordModelBase != null)
            {
                await _workFacade.RemoveAsync(workRecordModelBase.Id);
                Records.Remove(workRecordModelBase);
                _companyModelProvider.GetModel().WorkRecordsCount--;
                UpdateTotalPrices();
            }
			EndProcess();
		}

		private async Task EditAsync(IWorkRecordModelBase workRecordModelBase)
		{
			BeginProcess();
			await NavigationService.NavigateAsync("WorkRecordDetailPage", new NavigationParameters(){{"Record", workRecordModelBase} });
			EndProcess();
		}
	}
}