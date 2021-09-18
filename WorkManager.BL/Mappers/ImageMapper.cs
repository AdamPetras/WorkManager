using WorkManager.BL.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class ImageMapper:IMapper<ImageEntity,IImageModel>
	{
		private readonly IMapper<TaskEntity, ITaskModel> _taskMapper;

		public ImageMapper(IMapper<TaskEntity,ITaskModel> taskMapper)
		{
			_taskMapper = taskMapper;
		}

		public ImageEntity Map(IImageModel model)
		{
			return new ImageEntity()
			{
				Path = model.Path,
				Description = model.Description,
				TaskId = model.Task.Id,
				Task = _taskMapper.Map(model.Task)
			};
		}

		public IImageModel Map(ImageEntity entity)
		{
			return new ImageModel(entity.Id, entity.Path, entity.Description, _taskMapper.Map(entity.Task));
		}
	}
}