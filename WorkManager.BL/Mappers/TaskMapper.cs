using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
	public class TaskMapper : ITaskMapper
	{
        public TaskMapper()
		{
        }

		public TaskEntity Map(ITaskModel model)
		{
			if (model == null)
				return new TaskEntity();
			return new TaskEntity()
			{
				Id = model.Id,
				ActualDateTime = model.ActualDateTime,
				Name = model.Name,
				Description = model.Description,
				TaskDoneDateTime = model.TaskDoneDateTime,
				TaskGroupId = model.TaskGroupId,
				StateId = model.StateId,
				Priority = model.Priority.GetValue<EPriority>(),
				WorkTime = model.WorkTime,
			};
		}

		public ITaskModel Map(TaskEntity entity, int imagesCount)
		{
			if (entity == null)
				return new TaskModel();
			return new TaskModel(entity.Id, entity.ActualDateTime, entity.Name, imagesCount, entity.Description,
				entity.TaskDoneDateTime, entity.TaskGroupId, entity.StateId, entity.Priority,entity.WorkTime);
		}
    }
}