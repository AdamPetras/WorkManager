using System.Threading;
using System.Threading.Tasks;
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
				TaskId = model.TaskId,
			};
		}

		public IImageModel Map(ImageEntity entity)
		{
			return new ImageModel(entity.Id, entity.Path, entity.Description, entity.TaskId);
		}

        public Task<ImageEntity> MapAsync(IImageModel model, CancellationToken token)
        {
			return Task.FromResult(new ImageEntity()
            {
                Path = model.Path,
                Description = model.Description,
                TaskId = model.TaskId,
            });
		}

        public Task<IImageModel> MapAsync(ImageEntity entity, CancellationToken token)
        {
			return Task.FromResult<IImageModel>(new ImageModel(entity.Id, entity.Path, entity.Description, entity.TaskId));
        }
    }
}