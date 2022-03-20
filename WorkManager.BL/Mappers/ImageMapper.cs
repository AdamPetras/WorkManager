using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class ImageMapper : IImageMapper
	{

		public ImageMapper()
		{
		}

		public ImageEntity Map(IImageModel model)
		{
			return new ImageEntity()
			{
				Path = model.Path,
				Description = model.Description,
				TaskId = model.TaskId,
			};
		}

		public IImageModel Map(ImageEntity entity)
		{
			return new ImageModel(entity.Id, entity.Path, entity.Description, entity.TaskId);
		}
    }
}