using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.Enums;
using WorkManager.Extensions;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;

namespace WorkManager.ViewModels.Pages
{
    public class WorkRecordStatisticsPageViewModel : ViewModelBase
    {
        private readonly ICurrentModelProvider<ICompanyModel> _companyModelProvider;
        private readonly IWorkRecordFacade _workRecordFacade;

        public WorkRecordStatisticsPageViewModel(INavigationService navigationService,
            ICurrentModelProvider<ICompanyModel> companyModelProvider,
            IWorkRecordFacade workRecordFacade, ViewModelTaskExecute viewModelTaskExecute) : base(navigationService, viewModelTaskExecute)
        {
            _companyModelProvider = companyModelProvider;
            _workRecordFacade = workRecordFacade;
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

        private double _totalPrice;
        public double TotalPrice
        {
            get => _totalPrice;
            set
            {
                if (_totalPrice == value) return;
                _totalPrice = value;
                RaisePropertyChanged();
            }
        }

        private TimeSpan _totalHours;
        public TimeSpan TotalHours
        {
            get => _totalHours;
            set
            {
                if (_totalHours == value) return;
                _totalHours = value;
                RaisePropertyChanged();
            }
        }

        private double _totalPieces;
        public double TotalPieces
        {
            get => _totalPieces;
            set
            {
                if (_totalPieces == value) return;
                _totalPieces = value;
                RaisePropertyChanged();
            }
        }

        private double _totalRecordsThisYear;
        public double TotalRecordsThisYear
        {
            get => _totalRecordsThisYear;
            set
            {
                if (_totalRecordsThisYear == value) return;
                _totalRecordsThisYear = value;
                RaisePropertyChanged();
            }
        }

        private double _totalRecordsThisMonth;
        public double TotalRecordsThisMonth
        {
            get => _totalRecordsThisMonth;
            set
            {
                if (_totalRecordsThisMonth == value) return;
                _totalRecordsThisMonth = value;
                RaisePropertyChanged();
            }
        }

        private double _totalRecords;
        public double TotalRecords
        {
            get => _totalRecords;
            set
            {
                if (_totalRecords == value) return;
                _totalRecords = value;
                RaisePropertyChanged();
            }
        }

        public void SetDefault()
        {
            TotalPriceThisMonth = 0;
            TotalPriceThisYear = 0;
            TotalPrice = 0;
            TotalHours = TimeSpan.Zero;
            TotalPieces = 0;
            TotalRecordsThisYear = 0;
            TotalRecordsThisMonth = 0;
            TotalRecords = 0;
        }

        protected override async Task InitializeAsyncInt()
        {
            BeginProcess();
            await base.InitializeAsyncInt();
            SetDefault();
            DateTime today = DateTime.Today;
            foreach (IWorkRecordModelBase workRecordModel in await ViewModelTaskExecute.ExecuteTaskWithQueue(_companyModelProvider.GetModel().Id, _workRecordFacade.GetAllRecordsByCompanyAsync))
            {
                switch (workRecordModel.Type)
                {
                    case EWorkType.Time:
                        if (workRecordModel is IWorkTimeRecordModel timeModel)
                        {
                            CalculateHours(timeModel, today);
                        }
                        break;
                    case EWorkType.Piece:
                        if (workRecordModel is IWorkPiecesRecordModel piecesModel)
                        {
                            CalculatePieces(piecesModel,today);
                        }
                        break;
                    case EWorkType.Both:
                        if (workRecordModel is IWorkBothRecordModel bothModel)
                        {
                            CalculatePieces(bothModel, today);
                            CalculateHours(bothModel, today);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                CalculatePrices(workRecordModel, today);
                CalculateRecords(workRecordModel, today);
            }
            EndProcess();
        }

        private void CalculateRecords(IWorkRecordModelBase workRecordModel, DateTime today)
        {
            if (workRecordModel.ActualDateTime.Year == today.Year)
            {
                TotalRecordsThisYear++;
                if (workRecordModel.ActualDateTime.Month == today.Month)
                {
                    TotalRecordsThisMonth++;
                }
            }
            TotalRecords++;
        }

        private void CalculatePrices(IWorkRecordModelBase workRecordModel, DateTime today)
        {
            if (workRecordModel.ActualDateTime.Year == today.Year)
            {
                TotalPriceThisYear += workRecordModel.CalculatedPrice;
                if (workRecordModel.ActualDateTime.Month == today.Month)
                {
                    TotalPriceThisMonth += workRecordModel.CalculatedPrice;
                }
            }
            TotalPrice += workRecordModel.CalculatedPrice;
        }

        private void CalculateHours(IWorkTimeRecordModel workTimeRecordModel, DateTime today)
        {
            //if (workTimeRecordModel.ActualDateTime.Year == today.Year)
            //{
            //    if (workTimeRecordModel.ActualDateTime.Month == today.Month)
            //    {
            //        TotalPriceThisMonth++;
            //    }
            //}
            TotalHours = TotalHours.Add(workTimeRecordModel.WorkTime);
        }

        private void CalculatePieces(IWorkPiecesRecordModel workPiecesRecordModel, DateTime today)
        {
            TotalPieces += workPiecesRecordModel.Pieces;
        }
    }
}