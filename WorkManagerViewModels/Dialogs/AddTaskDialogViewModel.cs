using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.Enums;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;
using Xamarin.Essentials;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddTaskDialogViewModel : DialogViewModelBase
	{
		private readonly ITaskFacade _taskFacade;
		private readonly ICurrentModelProvider<ITaskGroupModel> _currentTaskGroupProvider;
		private readonly IKanbanTaskGroupFacade _kanbanTaskGroupFacade;
		private readonly IEventAggregator _eventAggregator;
		private readonly IToastMessageService _toastMessageService;
		private readonly IDialogService _dialogService;
		private readonly IImageFacade _imageFacade;

		public AddTaskDialogViewModel(INavigationService navigationService, ICurrentModelProvider<ITaskGroupModel> currentTaskGroupProvider, IKanbanTaskGroupFacade kanbanTaskGroupFacade,
			ITaskFacade taskFacade, IEventAggregator eventAggregator, IToastMessageService toastMessageService, IDialogService dialogService, IImageFacade imageFacade) : base(navigationService)
		{
			_currentTaskGroupProvider = currentTaskGroupProvider;
			_kanbanTaskGroupFacade = kanbanTaskGroupFacade;
			_eventAggregator = eventAggregator;
			_toastMessageService = toastMessageService;
			_dialogService = dialogService;
			_imageFacade = imageFacade;
			_taskFacade = taskFacade;
			CancelCommand = new DelegateCommand(Cancel);
			ConfirmCommand = new DelegateCommand(Confirm);
			TakePhotoCommand = new DelegateCommand(async () => await TakePhotoAsync());
			TaskStartDate = DateTime.Now;
			TaskDoneDate = DateTime.Now;
			PhotoPaths = new ObservableCollection<string>();
			DeletePhotoCommand = new DelegateCommand<string>(DeletePhoto);
		}

		public DelegateCommand<string> DeletePhotoCommand { get; }
		public DelegateCommand CancelCommand { get; }
		public DelegateCommand ConfirmCommand { get; }
		public DelegateCommand TakePhotoCommand { get; }

		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				if (_name == value) return;
				_name = value;
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

		private DateTime _taskStartDate;
		public DateTime TaskStartDate
		{
			get => _taskStartDate;
			set
			{
				if (_taskStartDate == value) return;
				_taskStartDate = value;
				RaisePropertyChanged();
			}
		}

		private DateTime _taskDoneDate;
		public DateTime TaskDoneDate
		{
			get => _taskDoneDate;
			set
			{
				if (_taskDoneDate == value) return;
				_taskDoneDate = value;
				RaisePropertyChanged();
			}
		}

		private EPriority _priority;
		public EPriority Priority
		{
			get => _priority;
			set
			{
				if (_priority == value) return;
				_priority = value;
				RaisePropertyChanged();
			}
		}

		private TimeSpan _workTime;
		public TimeSpan WorkTime
		{
			get => _workTime;
			set
			{
				if (_workTime == value) return;
				_workTime = value;
				RaisePropertyChanged();
			}
		}

		private ObservableCollection<string> _photoPaths;
		public ObservableCollection<string> PhotoPaths
		{
			get => _photoPaths;
			set
			{
				if (_photoPaths == value) return;
				_photoPaths = value;
				RaisePropertyChanged();
			}
		}

		private void Cancel()
		{
			OnRequestClose(null);
		}

		private void Confirm()
		{
			IKanbanStateModel firstKanban = _kanbanTaskGroupFacade
				.GetKanbansByTaskGroupId(_currentTaskGroupProvider.GetModel().Id).Single(s => s.Kanban.StateOrder == 0)
				.Kanban;
			ITaskModel model = new TaskModel(Guid.NewGuid(), TaskStartDate, Name, Description, TaskDoneDate,
				_currentTaskGroupProvider.GetModel(), firstKanban, Priority, WorkTime);
			_taskFacade.Add(model);
			foreach (string path in PhotoPaths)
			{
				_imageFacade.Add(new ImageModel(Guid.NewGuid(), path, "", model));
			}
			OnRequestClose(new DialogParameters(){{ "DialogEvent", new AddAfterDialogCloseDialogEvent<ITaskModel>(model)} });
		}

		private async Task TakePhotoAsync()
		{	
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("SelectPictureOrCaptureCameraDialog")).Parameters;
			string path = parameters.GetValue<string>("Path");
			if(path != null)
				PhotoPaths.Add(path);
		}

		private void DeletePhoto(string obj)
		{
			PhotoPaths.Remove(obj);
		}
	}
}