using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services.Dialogs;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Models;
using WorkManager.ViewModels.BaseClasses;
using WorkManager.ViewModels.Resources;
using Xamarin.Essentials;

namespace WorkManager.ViewModels.Dialogs
{
	public class SelectPictureOrCaptureCameraDialogViewModel : DialogViewModelBase
	{
		private readonly IToastMessageService _toastMessageService;
		private readonly IPhotoService _photoService;

		public SelectPictureOrCaptureCameraDialogViewModel(INavigationService navigationService, IToastMessageService toastMessageService, IPhotoService photoService) : base(navigationService)
		{
			_toastMessageService = toastMessageService;
			_photoService = photoService;
			SelectCameraCommand = new DelegateCommand(async () => await SelectCameraAsync());
			SelectGalleryCommand = new DelegateCommand(async () => await SelectGalleryAsync());
			CloseCommand = new DelegateCommand(Close);
		}

		public DelegateCommand SelectCameraCommand { get; }
		public DelegateCommand SelectGalleryCommand { get; }
		public DelegateCommand CloseCommand { get; }

		private async Task SelectGalleryAsync()
		{
			OnRequestClose(new DialogParameters(){{"Path", await _photoService.GalleryAsync()}});
		}

		private async Task SelectCameraAsync()
		{
			OnRequestClose(new DialogParameters(){{"Path", await _photoService.CameraAsync()}});
		}

		private void Close()
		{
			OnRequestClose(null);
		}
	}
}