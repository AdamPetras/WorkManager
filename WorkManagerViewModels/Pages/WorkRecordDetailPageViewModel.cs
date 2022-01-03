using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.Core;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class WorkRecordDetailPageViewModel : ViewModelBase

	{
		private readonly IWorkRecordModelFactory _workRecordModelFactory;
		private readonly IPageDialogService _pageDialogService;
		private readonly IWorkRecordFacade _workRecordFacade;

		public WorkRecordDetailPageViewModel(INavigationService navigationService, IWorkRecordModelFactory workRecordModelFactory, IPageDialogService pageDialogService, IWorkRecordFacade workRecordFacade) : base(navigationService)
		{
			_workRecordModelFactory = workRecordModelFactory;
			_pageDialogService = pageDialogService;
			_workRecordFacade = workRecordFacade;
			DeleteRecordCommand = new DelegateCommand(async () => await DeleteRecordAsync());
			SaveCommand = new DelegateCommand(async() => await Save());
		}

		public DelegateCommand DeleteRecordCommand { get; }
		public DelegateCommand SaveCommand { get; }


		private ObservableCollection<IWorkRecordModelBase> _recordModel;
		public ObservableCollection<IWorkRecordModelBase> RecordModel
		{
			get => _recordModel;
			set
			{
				if (_recordModel == value) return;
				_recordModel = value;
				RaisePropertyChanged();
			}
		}

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			BeginProcess();
			base.OnNavigatedToInt(parameters);
			//observable collection kvůli tomu, že binduju na datatemplate a tam je pouze itemsource
			RecordModel = new ObservableCollection<IWorkRecordModelBase>() { _workRecordModelFactory.CopyRecord(parameters.GetValue<IWorkRecordModelBase>("Record")) };  //vytváření nového modelu aby se neměnil model, který zde dojde pomocí navigace
			EndProcess();
		}

		private async Task DeleteRecordAsync()
		{
			IsDialogThrown = true;
			if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning, TranslateViewModelsSR.SelectedWorkRecordDeleteDialogMessage.Format(RecordModel.Single().ActualDateTime.ToString("dd.MM.yyyy")), TranslateViewModelsSR.DialogYes, TranslateViewModelsSR.DialogNo))
			{
				await _workRecordFacade.RemoveAsync(RecordModel.Single().Id);
				await NavigationService.GoBackAsync(new NavigationParameters() { { "DialogEvent", new RemoveAfterDialogCloseDialogEvent<IWorkRecordModelBase>(RecordModel.Single()) } });
			}
			IsDialogThrown = false;
		}

		private async Task Save()
		{
            IsDialogThrown = true;
			await _workRecordFacade.UpdateAsync(RecordModel.Single());
			await NavigationService.GoBackAsync(new NavigationParameters() { { "DialogEvent", new UpdateAfterDialogCloseDialogEvent<IWorkRecordModelBase>(RecordModel.Single()) } });
            IsDialogThrown = false;
		}
	}
}