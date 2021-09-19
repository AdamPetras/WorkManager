using System.Threading.Tasks;

namespace WorkManager.BL.Interfaces.Services
{
	public interface IPhotoService
	{
		Task<string> GalleryAsync();
		Task<string> CameraAsync();
	}
}