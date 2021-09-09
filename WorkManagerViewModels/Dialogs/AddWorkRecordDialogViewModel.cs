using System;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.DAL.Enums;
using WorkManager.Models;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddWorkRecordDialogViewModel : DialogViewModelBase
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly ICurrentModelProvider<ICompanyModel> _companyModelProvider;
		private readonly IFacade<IWorkRecordModelBase> _workRecordDetailFacade;
		private readonly IWorkRecordModelFactory _workRecordModelFactory;

		public AddWorkRecordDialogViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ICurrentModelProvider<ICompanyModel> companyModelProvider, IFacade<IWorkRecordModelBase> workRecordDetailFacade, IWorkRecordModelFactory workRecordModelFactory) : base(navigationService)
		{
			_eventAggregator = eventAggregator;
			_companyModelProvider = companyModelProvider;
			_workRecordDetailFacade = workRecordDetailFacade;
			_workRecordModelFactory = workRecordModelFactory;
			CancelCommand = new DelegateCommand(Cancel);
			ConfirmCommand = new DelegateCommand(Confirm);
			SetupDefaultValues();
		}

		public DelegateCommand CancelCommand { get; }
		public DelegateCommand ConfirmCommand { get; }

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
		
		private EWorkType _selectedWorkType;
		public EWorkType SelectedWorkType
		{
			get => _selectedWorkType;
			set
			{
				if (_selectedWorkType == value) return;
				_selectedWorkType = value;
				RaisePropertyChanged();
			}
		}

		private uint _pieces;
		public uint Pieces
		{
			get => _pieces;
			set
			{
				if (_pieces == value) return;
				_pieces = value;
				RaisePropertyChanged();
			}
		}

		private double _pricePerHour;
		public double PricePerHour
		{
			get => _pricePerHour;
			set
			{
				if (_pricePerHour == value) return;
				_pricePerHour = value;
				RaisePropertyChanged();
			}
		}

		private double _pricePerPiece;
		public double PricePerPiece
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

		private void SetupDefaultValues()
		{
			SelectedDate = DateTime.Today;
			WorkTime = TimeSpan.Zero;
			SelectedWorkType = EWorkType.Time;
			Pieces = 0;
			PricePerHour = 0;
			PricePerPiece = 0;
		}

		private void Confirm()
		{
			IWorkRecordModelBase model = _workRecordModelFactory.CreateWorkRecord(Guid.NewGuid(), SelectedDate, WorkTime,
				PricePerHour, Pieces, PricePerPiece, SelectedWorkType, Description, _companyModelProvider.GetModel());
			_workRecordDetailFacade.Add(model);
			OnRequestClose(new DialogParameters(){{ "DialogEvent", new AddAfterDialogCloseDialogEvent<IWorkRecordModelBase>(model) } });
		}

		private void Cancel()
		{
			OnRequestClose(null);
		}
	}
}