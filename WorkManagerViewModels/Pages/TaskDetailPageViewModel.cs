using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Core;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;

namespace WorkManager.ViewModels.Pages
{
	public class TaskDetailPageViewModel : ViewModelBase
	{
		private readonly IPageDialogService _pageDialogService;
		private readonly ITaskFacade _taskFacade;
		private readonly IDialogService _dialogService;
		private readonly IImageFacade _imageFacade;
		private readonly IToastMessageService _toastMessageService;

		public TaskDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ITaskFacade taskFacade, IDialogService dialogService,
			IImageFacade imageFacade, IToastMessageService toastMessageService) : base(navigationService)
		{
			_pageDialogService = pageDialogService;
			_taskFacade = taskFacade;
			_dialogService = dialogService;
			_imageFacade = imageFacade;
			_toastMessageService = toastMessageService;
			SaveCommand = new DelegateCommand(async () => await SaveAsync());
			DeletePhotoCommand = new DelegateCommand<IImageModel>(DeletePhoto);
			TakePhotoCommand = new DelegateCommand(async () => await TakePhotoAsync());
			ShowDetailImageDialogCommand = new DelegateCommand<string>(ShowDetailImageDialog);
			InitDialogCommands();
		}

		public DelegateCommand<IImageModel> DeletePhotoCommand { get; }
		public DelegateCommand SaveCommand { get; }
		public DelegateCommand DeleteTaskCommand { get; private set; }
		public DelegateCommand TakePhotoCommand { get; }
		public DelegateCommand<string> ShowDetailImageDialogCommand { get; }


		private ITaskModel _selectedTask;
		public ITaskModel SelectedTask
		{
			get => _selectedTask;
			private set
			{
				if (_selectedTask == value) return;
				_selectedTask = value;
				RaisePropertyChanged();
			}
		}

		private ObservableCollection<IImageModel> _photoPaths;
		public ObservableCollection<IImageModel> PhotoPaths
		{
			get => _photoPaths;
			set
			{
				if (_photoPaths == value) return;
				_photoPaths = value;
				RaisePropertyChanged();
			}
		}

		private ICollection<IImageModel> InitImages { get; set; }

		protected override void OnNavigatedToInt(INavigationParameters parameters)
		{
			base.OnNavigatedToInt(parameters);
			SelectedTask = new TaskModel(parameters.GetValue<ITaskModel>("Task"));  //vytváření nového modelu aby se neměnil model, který zde dojde pomocí navigace
			PhotoPaths = new ObservableCollection<IImageModel>(_imageFacade.GetAllImagesByTask(SelectedTask.Id));
			InitImages = PhotoPaths.ToList();
		}

		protected override void DestroyInt()
		{
			base.DestroyInt();
			DialogThrownEvent -= DeleteTaskCommand.RaiseCanExecuteChanged;
		}

		private void InitDialogCommands()
		{
			DeleteTaskCommand = new DelegateCommand(async () => await DeleteAsync(), () => !IsDialogThrown);
			DialogThrownEvent += DeleteTaskCommand.RaiseCanExecuteChanged;
		}

		private async Task SaveAsync()
		{
			await _taskFacade.UpdateAsync(SelectedTask);
			EnumerableDiffChecker<IImageModel> diffChecker = new EnumerableDiffChecker<IImageModel>();
			DifferentialCollection<IImageModel> collections = diffChecker.CheckCollectionDifference(InitImages, PhotoPaths);
			foreach (IImageModel imageModel in collections.AddEnumerable)
			{
				await _imageFacade.AddAsync(imageModel);
			}
			foreach (IImageModel imageModel in collections.DeleteEnumerable)
			{
				await _imageFacade.RemoveAsync(imageModel.Id);
			}
			await NavigationService.GoBackAsync(new NavigationParameters() { { "DialogEvent", new UpdateAfterDialogCloseDialogEvent<ITaskModel>(SelectedTask) } });
		}

		private async Task DeleteAsync()
		{
			IsDialogThrown = true;
			if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning, TranslateViewModelsSR.SelectedTaskDeleteMessageFormat(SelectedTask.Name), TranslateViewModelsSR.DialogYes, TranslateViewModelsSR.DialogNo))
			{
				await _taskFacade.RemoveAsync(SelectedTask.Id);
				await NavigationService.GoBackAsync(new NavigationParameters() { { "DialogEvent", new RemoveAfterDialogCloseDialogEvent<ITaskModel>(SelectedTask) } });
			}
			IsDialogThrown = false;
		}

		private async Task TakePhotoAsync()
		{
			IDialogParameters parameters = (await _dialogService.ShowDialogAsync("SelectPictureOrCaptureCameraDialog")).Parameters;
			string path = parameters.GetValue<string>("Path");
			if (path != null)
			{
				PhotoPaths.Add(new ImageModel(Guid.NewGuid(), path, "", SelectedTask));
			}
		}

		private void DeletePhoto(IImageModel obj)
		{
			PhotoPaths.Remove(obj);
		}

		private void ShowDetailImageDialog(string photoPath)
		{
			_dialogService.ShowDialog("ImageDetailDialog", new DialogParameters() { { "Path", photoPath } });
		}
	}
}