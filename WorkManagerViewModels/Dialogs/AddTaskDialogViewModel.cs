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
using WorkManager.BL.NavigationParams;
using WorkManager.DAL.Enums;
using WorkManager.Extensions;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;
using WorkManager.Xamarin.Core;
using Xamarin.Essentials;

namespace WorkManager.ViewModels.Dialogs
{
	public class AddTaskDialogViewModel : ConfirmDialogViewModelBase
	{
		private readonly ITaskFacade _taskFacade;
		private readonly ICurrentModelProvider<ITaskGroupModel> _currentTaskGroupProvider;
		private readonly IDialogService _dialogService;
		private readonly IImageFacade _imageFacade;
        private readonly IRelatedTaskFacade _relatedTaskFacade;

        private IKanbanStateModel _selectedKanbanState;

		public AddTaskDialogViewModel(INavigationService navigationService,
            ICurrentModelProvider<ITaskGroupModel> currentTaskGroupProvider,
            ITaskFacade taskFacade, IDialogService dialogService, IImageFacade imageFacade, IRelatedTaskFacade relatedTaskFacade,
            ViewModelTaskExecute viewModelTaskExecute) : base(navigationService, viewModelTaskExecute)
		{
			_currentTaskGroupProvider = currentTaskGroupProvider;
			_dialogService = dialogService;
			_imageFacade = imageFacade;
            _relatedTaskFacade = relatedTaskFacade;
            _taskFacade = taskFacade;
			TakePhotoCommand = new DelegateCommand(async () => await TakePhotoAsync());
			TaskStartDate = DateTime.Now;
			Priority = new LocalizedEnum(EPriority.None);
			TaskDoneDate = DateTime.Now;
			PhotoPaths = new ObservableCollection<string>();
			DeletePhotoCommand = new DelegateCommand<string>(DeletePhoto);
			ShowDetailImageDialogCommand = new DelegateCommand<string>(ShowDetailImageDialog);
            NameMaxLength = typeof(ITaskModel).GetStringMaxLength(nameof(ITaskModel.Name));
            DescriptionMaxLength = typeof(ITaskModel).GetStringMaxLength(nameof(ITaskModel.Description));
		}

        public DelegateCommand<string> DeletePhotoCommand { get; }
        public DelegateCommand TakePhotoCommand { get; }
		public DelegateCommand<string> ShowDetailImageDialogCommand { get; }

		public int NameMaxLength { get; }
		public int DescriptionMaxLength { get; }

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

		private LocalizedEnum _priority;
		public LocalizedEnum Priority
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

		protected override async Task ConfirmAsyncInt()
        {
			BeginProcess();
            IRelatedTaskModel newRelatedTaskModel = new RelatedTaskModel(Guid.NewGuid(), "Test", new List<ITaskModel>());
            ITaskModel model = new TaskModel(Guid.NewGuid(), TaskStartDate, Name, 0, Description, TaskDoneDate,
                _currentTaskGroupProvider.GetModel().Id, _selectedKanbanState.Id, Priority.GetValue<EPriority>(), WorkTime, newRelatedTaskModel.Id);

			await ViewModelTaskExecute.ExecuteTaskWithQueue(async (token) =>
            {
                await _relatedTaskFacade.AddAsync(newRelatedTaskModel, token);
                await _taskFacade.AddAsync(model, token);
                foreach (string path in PhotoPaths)
                {
                    await _imageFacade.AddAsync(new ImageModel(Guid.NewGuid(), path, "", model.Id), token);
                }
			});
            
            _currentTaskGroupProvider.GetModel().TasksCount++;
            OnRequestClose(new DialogParameters() { { "DialogEvent", new AddAfterDialogCloseDialogEvent<ITaskModel>(model) } });
            EndProcess();
		}

        protected override void OnDialogOpenedInt(IDialogParameters parameters)
        {
            base.OnDialogOpenedInt(parameters);
            KanbanStateNavigationParameters navParameters = new KanbanStateNavigationParameters(parameters);
            if (navParameters.KanbanState != null)
                _selectedKanbanState = navParameters.KanbanState;
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

		private void ShowDetailImageDialog(string photoPath)
		{
			_dialogService.ShowDialog("ImageDetailDialog", new DialogParameters() { { "Path", photoPath } });
		}
	}
}