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
using WorkManager.BL.Services;
using WorkManager.Core;
using WorkManager.DAL.Enums;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class WorkRecordPageViewModel: ViewModelBase
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly ICurrentModelProvider<ICompanyModel> _companyModelProvider;
		private readonly IRecordTotalCalculatorService _recordTotalCalculatorService;
		private readonly IWorkRecordFacade _workFacade;
		private readonly IDialogService _dialogService;
		private readonly DialogEventService _dialogEventService;
		private readonly IPageDialogService _pageDialogService;

		private EFilterType _selectedFilter;
		private IWorkRecordModelBase _selectedRecord;

		public WorkRecordPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ICurrentModelProvider<ICompanyModel> companyModelProvider, 
			IRecordTotalCalculatorService recordTotalCalculatorService, IWorkRecordFacade workFacade, IDialogService dialogService, DialogEventService dialogEventService, IPageDialogService pageDialogService) : base(navigationService)
		{
			_eventAggregator = eventAggregator;
			_companyModelProvider = companyModelProvider;
			_recordTotalCalculatorService = recordTotalCalculatorService;
			_workFacade = workFacade;
			_dialogService = dialogService;
			_dialogEventService = dialogEventService;
			_pageDialogService = pageDialogService;
			_selectedFilter = EFilterType.ThisMonth;
			FilteredRecords ??= new FilteredObservableCollection<IWorkRecordModelBase>(_workFacade.GetAllRecordsByCompany(companyModelProvider.GetModel().Id, EFilterType.None).OrderByDescending(s=>s.ActualDateTime), CreateFilterByEnum(_selectedFilter));
			ShowAddDialogCommand = new DelegateCommand(async()=> await ShowAddDialogAsync());
			ShowStatisticsCommand = new DelegateCommand(ShowStatistics);
			ShowFilterDialogCommand = new DelegateCommand(async()=>await ShowFilterDialog());
			ClearWholeOrDeleteSingleRecordCommand =
				new DelegateCommand(async () => ClearWholeOrDeleteSingleRecordAsync());
			SelectedRecordCommand = new DelegateCommand<IWorkRecordModelBase>(SelectedRecord);
		}

		public DelegateCommand<IWorkRecordModelBase> SelectedRecordCommand { get; }
		public DelegateCommand ClearWholeOrDeleteSingleRecordCommand { get; }
		public DelegateCommand ShowFilterDialogCommand { get; }
		public DelegateCommand ShowStatisticsCommand { get; }
		public DelegateCommand ShowAddDialogCommand { get; }

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

		public double TotalPriceThisMonth => _recordTotalCalculatorService.CalculateThisMonth(FilteredRecords.WholeCollection);

		public double TotalPriceThisYear => _recordTotalCalculatorService.CalculateThisYear(FilteredRecords.WholeCollection);

		private async Task ShowAddDialogAsync()
		{
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("AddWorkRecordDialogView")).Parameters;
			IDialogEvent dialogEvent = parameters.GetValue<IDialogEvent>("DialogEvent");
			_dialogEventService.OnRaiseDialogEvent(dialogEvent, FilteredRecords.WholeCollection);
			RaisePropertyChanged(nameof(TotalPriceThisMonth));
			RaisePropertyChanged(nameof(TotalPriceThisYear));
			FilteredRecords.WholeCollection = new ObservableCollection<IWorkRecordModelBase>(FilteredRecords.WholeCollection.OrderByDescending(s => s.ActualDateTime));
		}

		private void ShowStatistics()
		{
		}

		private async Task ShowFilterDialog()
		{
			IDialogParameters parameters =
				(await _dialogService.ShowDialogAsync("FilterDialogView",
					new DialogParameters() {{"Filter", _selectedFilter}})).Parameters;
			if(parameters.Any())	//parameters je typ enumerable tudíž rychlejší přístup je využít Any() namísto Count() == 0
				_selectedFilter = parameters.GetValue<EFilterType>("Filter");
			FilteredRecords.Filter = CreateFilterByEnum(_selectedFilter);
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			if(FilteredRecords.WholeCollection.Count == 0)
				FilteredRecords = new FilteredObservableCollection<IWorkRecordModelBase>(_workFacade.GetAllRecordsByCompany(_companyModelProvider.GetModel().Id, EFilterType.None).OrderByDescending(s => s.ActualDateTime), CreateFilterByEnum(_selectedFilter));
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

		private async Task ClearWholeOrDeleteSingleRecordAsync()
		{
			if (_selectedRecord != null)
			{
				await _workFacade.RemoveAsync(_selectedRecord.Id);
				FilteredRecords.WholeCollection.Remove(_selectedRecord);
			}
			else
			{
				if (await _pageDialogService.DisplayAlertAsync(WorkRecordPageViewModelSR.ClearDialogTitle,
					WorkRecordPageViewModelSR.ClearDialogMessage, WorkRecordPageViewModelSR.DialogYes,
					WorkRecordPageViewModelSR.DialogNo))
				{
					_workFacade.Clear();
					FilteredRecords.WholeCollection?.Clear();
				}
			}
		}

		private void SelectedRecord(IWorkRecordModelBase obj)
		{
			_selectedRecord = obj;
		}
	}
}