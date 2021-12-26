using System;
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
	public class WorkRecordPageViewModel : ViewModelBase
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
            _filterDateFrom = DateTime.Today;
            _filterDateTo = DateTime.Today.AddDays(31);
			EditCommand = new DelegateCommand<IWorkRecordModelBase>(async (s) => await EditAsync(s));
            DeleteRecordCommand = new DelegateCommand<IWorkRecordModelBase>(async (s) => await DeleteRecordAsync(s));
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());
			InitDialogCommands();
		}

        public DelegateCommand<IWorkRecordModelBase> SelectedRecordCommand { get; }
        public DelegateCommand RefreshCommand { get; }
		public DelegateCommand ClearRecordsCommand { get; private set; }
		public DelegateCommand<IWorkRecordModelBase> DeleteRecordCommand { get; private set; }
		public DelegateCommand ShowFilterDialogCommand { get; private set; }
		public DelegateCommand ShowStatisticsCommand { get; private set; }
		public DelegateCommand<IWorkRecordModelBase> EditCommand { get; }
		public DelegateCommand ShowAddDialogCommand { get; private set; }

		private FilteredObservableCollection<IWorkRecordModelBase> _filteredRecords;

		public FilteredObservableCollection<IWorkRecordModelBase> FilteredRecords
		{
			get => _filteredRecords;
			set
			{
				if (_filteredRecords == value) return;
				_filteredRecords = value;
				RaisePropertyChanged();
			}
		}

		public double TotalPriceThisMonth => FilteredRecords?.WholeCollection == null ? 0 : _recordTotalCalculatorService.CalculateThisMonth(FilteredRecords?.WholeCollection);

		public double TotalPriceThisYear => FilteredRecords?.WholeCollection == null ? 0 : _recordTotalCalculatorService.CalculateThisYear(FilteredRecords?.WholeCollection);

        protected override async Task InitializeAsyncInt()
		{
			await base.InitializeAsyncInt();
            await RefreshAsync();
			UpdateTotalPrices();
		}

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= ShowAddDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ShowStatisticsCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ShowFilterDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ClearRecordsCommand.RaiseCanExecuteChanged;
		}

		protected override async void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
            await RefreshAsync();
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, FilteredRecords.WholeCollection);
			UpdateTotalPrices();
		}

		private void UpdateTotalPrices()
		{
			RaisePropertyChanged(nameof(TotalPriceThisMonth));
			RaisePropertyChanged(nameof(TotalPriceThisYear));
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
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, FilteredRecords.WholeCollection);
			UpdateTotalPrices();
			FilteredRecords.WholeCollection = new System.Collections.ObjectModel.ObservableCollection<IWorkRecordModelBase>(FilteredRecords.WholeCollection.OrderByDescending(s => s.ActualDateTime));
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
            IDialogParameters parameters = (await _dialogService.ShowDialogAsync("FilterDialogView", new FilterNavigationParameters(_filterDateFrom, _filterDateTo))).Parameters;
            if (parameters.Any()) //parameters je typ enumerable tudíž rychlejší přístup je využít Any() namísto Count() == 0
            {
				FilterNavigationParameters filterParams = (FilterNavigationParameters) parameters;
                _filterDateFrom = filterParams.DateFrom;
                _filterDateTo = filterParams.DateTo;
            }
			FilteredRecords.Filter = CreateFilterByDate(_filterDateFrom,_filterDateTo);
			IsDialogThrown = false;
		}

        private Func<IWorkRecordModelBase, bool> CreateFilterByDate(DateTime dateFrom, DateTime dateTo)
        {
            return s => s.ActualDateTime.IsBetween(dateFrom, dateTo);
        }

        private Func<IWorkRecordModelBase, bool> CreateFilterByEnum(EFilterType filterType)
		{
			return filterType switch
			{
				EFilterType.None => (s) => true,
				EFilterType.ThisYear => (s) => s.ActualDateTime.Year == DateTime.Today.Year,
				EFilterType.ThisMonth => (s) =>
					s.ActualDateTime.Year == DateTime.Today.Year && s.ActualDateTime.Month == DateTime.Today.Month,
				_ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null)
			};
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
				FilteredRecords.WholeCollection?.Clear();
			}
			IsDialogThrown = false;
			RaisePropertyChanged(nameof(TotalPriceThisMonth));
			RaisePropertyChanged(nameof(TotalPriceThisYear));
			EndProcess();
		}

        private async Task RefreshAsync()
        {
			BeginProcess();
            FilteredRecords = new FilteredObservableCollection<IWorkRecordModelBase>((await _workFacade.GetAllRecordsByCompanyAsync(_companyModelProvider.GetModel().Id, EFilterType.None)).OrderByDescending(s => s.ActualDateTime), CreateFilterByDate(_filterDateFrom,_filterDateTo));
			EndProcess();
        }

		private async Task DeleteRecordAsync(IWorkRecordModelBase workRecordModelBase)
        {
			BeginProcess();
            if (workRecordModelBase != null)
            {
                await _workFacade.RemoveAsync(workRecordModelBase.Id);
                FilteredRecords.WholeCollection.Remove(workRecordModelBase);
                RaisePropertyChanged(nameof(TotalPriceThisMonth));
                RaisePropertyChanged(nameof(TotalPriceThisYear));
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