using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.Enums;
using WorkManager.Extensions;
using WorkManager.Models;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.Xamarin.Core;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddWorkRecordDialogViewModel : ConfirmDialogViewModelBase
	{
		private readonly ICurrentModelProvider<ICompanyModel> _companyModelProvider;
		private readonly IFacade<IWorkRecordModelBase> _workRecordDetailFacade;
		private readonly IWorkRecordModelFactory _workRecordModelFactory;
        private readonly IToastMessageService _toastMessageService;

        public AddWorkRecordDialogViewModel(INavigationService navigationService,
            ICurrentModelProvider<ICompanyModel> companyModelProvider,
            IFacade<IWorkRecordModelBase> workRecordDetailFacade, IWorkRecordModelFactory workRecordModelFactory,
            ViewModelTaskExecute viewModelTaskExecute, IToastMessageService toastMessageService) : base(navigationService, viewModelTaskExecute)
		{
			_companyModelProvider = companyModelProvider;
			_workRecordDetailFacade = workRecordDetailFacade;
			_workRecordModelFactory = workRecordModelFactory;
            _toastMessageService = toastMessageService;
            SetupDefaultValues();
            DescriptionMaxLength = typeof(IWorkRecordModelBase).GetStringMaxLength(nameof(IWorkRecordModelBase.Description));
		}

		public int DescriptionMaxLength { get; }

        private DateTime _selectedDate;
		public DateTime SelectedDate
		{
			get => _selectedDate;
			set
			{
				_selectedDate = value;
				RaisePropertyChanged();
			}
		}

		private TimeSpan _workTime;
		public TimeSpan WorkTime
		{
			get => _workTime;
			set
			{
				_workTime = value;
				RaisePropertyChanged();
			}
		}
		
		private LocalizedEnum _selectedWorkType;
		public LocalizedEnum SelectedWorkType
		{
			get => _selectedWorkType;
			set
			{
				if (_selectedWorkType == value) return;
				_selectedWorkType = value;
				RaisePropertyChanged();
			}
		}

		private uint? _pieces;
		public uint? Pieces
		{
			get => _pieces;
			set
			{
				if (_pieces == value) return;
				_pieces = value;
				RaisePropertyChanged();
			}
		}

		private double? _pricePerHour;
		public double? PricePerHour
		{
			get => _pricePerHour;
			set
			{
				if (_pricePerHour == value) return;
				_pricePerHour = value;
				RaisePropertyChanged();
			}
		}

		private double? _pricePerPiece;
		public double? PricePerPiece
		{
			get => _pricePerPiece;
			set
			{
				if (_pricePerPiece == value) return;
				_pricePerPiece = value;
				RaisePropertyChanged();
			}
		}

		private string _description;
		public string Description
		{
			get => _description;
			set
			{
				if (_description == value) return;
				_description = value;
				RaisePropertyChanged();
			}
		}

        protected override async Task ConfirmAsyncInt()
        {
            double parsedPricePerHour = 0;
            uint parsedPieces = 0;
            double parsedPricePerPiece = 0;
            if (PricePerHour != null)
                parsedPricePerHour = PricePerHour.Value;
            if (PricePerPiece != null)
                parsedPricePerPiece = PricePerPiece.Value;
            if (Pieces != null)
                parsedPieces = Pieces.Value;
            IWorkRecordModelBase model = _workRecordModelFactory.CreateWorkRecord(Guid.NewGuid(), SelectedDate, WorkTime,
                parsedPricePerHour, parsedPieces, parsedPricePerPiece, SelectedWorkType.GetValue<EWorkType>(), Description, _companyModelProvider.GetModel().Id);
            await ViewModelTaskExecute.ExecuteTaskWithQueue(model, _workRecordDetailFacade.AddAsync);
            _companyModelProvider.GetModel().WorkRecordsCount++;
            OnRequestClose(new DialogParameters() { { "DialogEvent", new AddAfterDialogCloseDialogEvent<IWorkRecordModelBase>(model) } });
		}

		private void SetupDefaultValues()
		{
			SelectedDate = DateTime.Today;
			WorkTime = TimeSpan.Zero;
			SelectedWorkType = new LocalizedEnum(EWorkType.Time);
			Pieces = null;
			PricePerHour = null;
			PricePerPiece = null;
		}
	}
}