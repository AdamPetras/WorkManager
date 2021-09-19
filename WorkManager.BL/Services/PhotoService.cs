using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using WorkManager.BL.Interfaces.Services;
using WorkManager.BL.Resources;
using Xamarin.Essentials;

namespace WorkManager.BL.Services
{
	public class PhotoService : IPhotoService
	{
		private readonly IToastMessageService _toastMessageService;

		public PhotoService(IToastMessageService toastMessageService)
		{
			_toastMessageService = toastMessageService;
		}

		public async Task<string> GalleryAsync()
		{
			try
			{
				FileResult photo = await MediaPicker.PickPhotoAsync();
				return photo.FullPath;
			}
			catch (PermissionException pEx)
			{
				_toastMessageService.LongAlert(TranslateBussinessSR.AplicationHaveNoPermissionsToUseFormat(TranslateBussinessSR.Camera));
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}
			return string.Empty;
		}

		public async Task<string> CameraAsync()
		{
			try
			{
				FileResult photo = await MediaPicker.CapturePhotoAsync();
				return await LoadPhotoAsync(photo);
			}
			catch (FeatureNotSupportedException fnsEx)
			{
				_toastMessageService.LongAlert(TranslateBussinessSR.FeatureIsNotSupportedFormat(TranslateBussinessSR.Camera));
			}
			catch (PermissionException pEx)
			{
				_toastMessageService.LongAlert(TranslateBussinessSR.AplicationHaveNoPermissionsToUseFormat(TranslateBussinessSR.Camera));
			}
			catch (Exception ex)
			{
				Crashes.TrackError(ex);
			}
			return string.Empty;
		}

		private async Task<string> LoadPhotoAsync(FileResult photo)
		{
			if (photo == null)
			{
				return null;
			}
			var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
			await using (Stream stream = await photo.OpenReadAsync())   //IAsyncDisposable proč nevyužít await using...
			{
				await using (FileStream newStream = File.OpenWrite(newFile))
				{
					await stream.CopyToAsync(newStream);
				}
			}
			return newFile;
		}
	}
}