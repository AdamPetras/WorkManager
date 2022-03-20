using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
    public interface IImageMapper : IMapper<ImageEntity, IImageModel>
    {
        IImageModel Map(ImageEntity entity);
    }
}