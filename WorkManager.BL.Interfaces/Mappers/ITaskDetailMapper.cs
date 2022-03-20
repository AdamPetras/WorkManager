using System.Collections.Generic;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Mappers
{
    public interface ITaskDetailMapper : IMapper<TaskEntity, ITaskDetailModel>
    {
        ITaskDetailModel Map(TaskEntity entity, int imagesCount);
    }
}