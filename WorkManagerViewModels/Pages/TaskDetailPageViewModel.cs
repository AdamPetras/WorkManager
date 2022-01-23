using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using WorkManager.BL.DialogEvents;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.NavigationParams;
using WorkManager.Core;
using WorkManager.Extensions;
using WorkManager.Models;
using WorkManager.Models.Interfaces;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;
using Xamarin.Forms;

namespace WorkManager.ViewModels.Pages
{
	public class TaskDetailPageViewModel : ViewModelBase
	{
		private readonly IPageDialogService _pageDialogService;
		private readonly ITaskFacade _taskFacade;
		private readonly IDialogService _dialogService;
		private readonly IImageFacade _imageFacade;

        private ITaskModel _defaultTask;

		public TaskDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ITaskFacade taskFacade, IDialogService dialogService, IImageFacade imageFacade) : base(navigationService)
		{
			_pageDialogService = pageDialogService;
			_taskFacade = taskFacade;
			_dialogService = dialogService;
			_imageFacade = imageFacade;
			SaveCommand = new DelegateCommand(async () => await SaveAsync());
			DeletePhotoCommand = new DelegateCommand<IImageModel>(DeletePhoto);
			TakePhotoCommand = new DelegateCommand(async () => await TakePhotoAsync());
            GoBackCommand = new DelegateCommand(async () => await GoBackAsync());
            ShowDetailImageDialogCommand = new DelegateCommand<string>(ShowDetailImageDialog);
            PhotoPaths = new ObservableCollection<IImageModel>();
			InitDialogCommands();
            NameMaxLength = typeof(ITaskModel).GetStringMaxLength(nameof(ITaskModel.Name));
            DescriptionMaxLength = typeof(ITaskModel).GetStringMaxLength(nameof(ITaskModel.Description));
        }

		public DelegateCommand<IImageModel> DeletePhotoCommand { get; }
		public DelegateCommand SaveCommand { get; }
		public DelegateCommand DeleteTaskCommand { get; private set; }
		public DelegateCommand TakePhotoCommand { get; }
		public DelegateCommand<string> ShowDetailImageDialogCommand { get; }
        public DelegateCommand GoBackCommand { get; }

        public int NameMaxLength { get; }
        public int DescriptionMaxLength { get; }

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


        protected override async Task OnNavigatedToAsyncInt(INavigationParameters parameters)
		{
			await base.OnNavigatedToAsyncInt(parameters);
            if (!parameters.Any())
            {
                return;
            }
            BeginProcess();
			TaskNavigationParameters navigationParameters = new TaskNavigationParameters(parameters);
			SelectedTask = navigationParameters.TaskModel;
			_defaultTask = new TaskModel(navigationParameters.TaskModel);
			PhotoPaths = await _imageFacade.GetAllImagesByTaskAsync(SelectedTask.Id).ToObservableCollectionAsync();
			InitImages = PhotoPaths.ToList();
			EndProcess();
		}

		private async Task GoBackAsync()
        {
            if (!_defaultTask.Equals(SelectedTask))
            {
                if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleInfo, TranslateViewModelsSR.DoYouWantToSaveDialogMessage, TranslateViewModelsSR.DialogYes, TranslateViewModelsSR.DialogNo))
                    await SaveAsync();
            }
            await NavigationService.GoBackAsync();
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
			if (await _pageDialogService.DisplayAlertAsync(TranslateViewModelsSR.DialogTitleWarning, TranslateViewModelsSR.SelectedTaskDeleteMessage.Format(SelectedTask.Name), TranslateViewModelsSR.DialogYes, TranslateViewModelsSR.DialogNo))
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
			if (!string.IsNullOrWhiteSpace(path))
			{
				PhotoPaths.Add(new ImageModel(Guid.NewGuid(), path, "", SelectedTask.Id));
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