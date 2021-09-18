using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class ImageFacade : FacadeBase<IImageModel, ImageEntity>, IImageFacade
	{
		public new IImageRepository Repository;
		public ImageFacade(IImageRepository repository, IMapper<ImageEntity, IImageModel> mapper) : base(repository, mapper)
		{
			Repository = repository;
		}
	}
}