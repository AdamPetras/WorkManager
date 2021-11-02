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
	public class WorkRecordPageViewModel : ViewModelBase
	{
		private readonly ICurrentModelProvider<ICompanyModel> _companyModelProvider;
		private readonly IRecordTotalCalculatorService _recordTotalCalculatorService;
		private readonly IWorkRecordFacade _workFacade;
		private readonly IDialogService _dialogService;
		private readonly DialogEventService _dialogEventService;
		private readonly IPageDialogService _pageDialogService;

		private EFilterType _selectedFilter;

		public WorkRecordPageViewModel(INavigationService navigationService, ICurrentModelProvider<ICompanyModel> companyModelProvider, IRecordTotalCalculatorService recordTotalCalculatorService,
			IWorkRecordFacade workFacade, IDialogService dialogService, DialogEventService dialogEventService, IPageDialogService pageDialogService) : base(navigationService)
		{
			_companyModelProvider = companyModelProvider;
			_recordTotalCalculatorService = recordTotalCalculatorService;
			_workFacade = workFacade;
			_dialogService = dialogService;
			_dialogEventService = dialogEventService;
			_pageDialogService = pageDialogService;
			_selectedFilter = EFilterType.ThisMonth;
			SelectedRecordCommand = new DelegateCommand<IWorkRecordModelBase>(SelectRecord);
			EditCommand = new DelegateCommand(async () => await EditAsync(), () => SelectedRecord != null);
			InitDialogCommands();
		}

		public DelegateCommand<IWorkRecordModelBase> SelectedRecordCommand { get; }
		public DelegateCommand ClearWholeOrDeleteSingleRecordCommand { get; private set; }
		public DelegateCommand ShowFilterDialogCommand { get; private set; }
		public DelegateCommand ShowStatisticsCommand { get; private set; }
		public DelegateCommand EditCommand { get; }
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

		private IWorkRecordModelBase _selectedRecord;
		public IWorkRecordModelBase SelectedRecord
		{
			get => _selectedRecord;
			set
			{
				if (_selectedRecord == value) return;
				_selectedRecord = value;
				RaisePropertyChanged();
				EditCommand.RaiseCanExecuteChanged();
			}
		}

		public double TotalPriceThisMonth => FilteredRecords?.WholeCollection == null ? 0 : _recordTotalCalculatorService.CalculateThisMonth(FilteredRecords?.WholeCollection);

		public double TotalPriceThisYear => FilteredRecords?.WholeCollection == null ? 0 : _recordTotalCalculatorService.CalculateThisYear(FilteredRecords?.WholeCollection);


		protected override void InitializeInt()
		{
			base.InitializeInt();
			FilteredRecords ??= new FilteredObservableCollection<IWorkRecordModelBase>(_workFacade.GetAllRecordsByCompany(_companyModelProvider.GetModel().Id, EFilterType.None).OrderByDescending(s => s.ActualDateTime), CreateFilterByEnum(_selectedFilter));
			UpdateTotalPrices();
		}

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= ShowAddDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ShowStatisticsCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ShowFilterDialogCommand.RaiseCanExecuteChanged;
			DialogThrownEvent -= ClearWholeOrDeleteSingleRecordCommand.RaiseCanExecuteChanged;
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			FilteredRecords ??= new FilteredObservableCollection<IWorkRecordModelBase>(_workFacade.GetAllRecordsByCompany(_companyModelProvider.GetModel().Id, EFilterType.None).OrderByDescending(s => s.ActualDateTime), CreateFilterByEnum(_selectedFilter));
			if (FilteredRecords?.WholeCollection.Count == 0)
				FilteredRecords.WholeCollection = new System.Collections.ObjectModel.ObservableCollection<IWorkRecordModelBase>(_workFacade.GetAllRecordsByCompany(_companyModelProvider.GetModel().Id, EFilterType.None)
					.OrderByDescending(s => s.ActualDateTime));
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
			ClearWholeOrDeleteSingleRecordCommand = new DelegateCommand(async () => await ClearWholeOrDeleteSingleRecordAsync(), () => !IsDialogThrown);
			DialogThrownEvent += ClearWholeOrDeleteSingleRecordCommand.RaiseCanExecuteChanged;
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
			IDialogParameters parameters =
				(await _dialogService.ShowDialogAsync("FilterDialogView",
					new DialogParameters() { { "Filter", _selectedFilter } })).Parameters;
			if (parameters.Any())   //parameters je typ enumerable tudíž rychlejší přístup je využít Any() namísto Count() == 0
				_selectedFilter = parameters.GetValue<EFilterType>("Filter");
			FilteredRecords.Filter = CreateFilterByEnum(_selectedFilter);
			IsDialogThrown = false;
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
			BeginProcess();
			if (_selectedRecord != null)
			{
				if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
					TranslateViewModelsSR.SelectedWorkRecordDeleteDialogMessageFormat(_selectedRecord.ActualDateTime.ToString("dd.MM.yyyy")), TranslateViewModelsSR.DialogYes,
					TranslateViewModelsSR.DialogNo))
				{
					await _workFacade.RemoveAsync(_selectedRecord.Id);
					FilteredRecords.WholeCollection.Remove(_selectedRecord);
					_selectedRecord = null;
				}
			}
			else
			{
				IsDialogThrown = true;
				if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning,
					TranslateViewModelsSR.WorkRecordClearDialogMessage, TranslateViewModelsSR.DialogYes,
					TranslateViewModelsSR.DialogNo))
				{
					await _workFacade.ClearAsync();
					FilteredRecords.WholeCollection?.Clear();
				}
				IsDialogThrown = false;
			}
			RaisePropertyChanged(nameof(TotalPriceThisMonth));
			RaisePropertyChanged(nameof(TotalPriceThisYear));
			EndProcess();
		}

		private void SelectRecord(IWorkRecordModelBase obj)
		{
			SelectedRecord = obj;
		}

		private async Task EditAsync()
		{
			BeginProcess();
			await NavigationService.NavigateAsync("WorkRecordDetailPage", new NavigationParameters(){{"Record", SelectedRecord}});
			EndProcess();
		}
	}
}