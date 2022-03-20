using System.Linq;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.DAL.Entities;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
    public class RelatedTaskMapper : IRelatedTaskMapper
    {
        private readonly ITaskMapper _taskMapper;

        public RelatedTaskMapper(ITaskMapper taskMapper)
        {
            _taskMapper = taskMapper;
        }

        public RelatedTaskEntity Map(IRelatedTaskModel model)
        {
            return new RelatedTaskEntity()
            {
                Id = model.Id,
                Name = model.Name,
                RelatedTasks = model.RelatedBy.Select(_taskMapper.Map).ToList()
            };
        }

        public IRelatedTaskModel Map(RelatedTaskEntity item)
        {
            return new RelatedTaskModel(item.Id, item.Name, item.RelatedTasks.Select(_taskMapper.Map).ToList());
        }
    }
}