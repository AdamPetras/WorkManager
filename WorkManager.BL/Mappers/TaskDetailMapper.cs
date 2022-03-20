using System.Collections.Generic;
using System.Linq;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;
using WorkManager.Models;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Mappers
{
    public class TaskDetailMapper : ITaskDetailMapper
    {
        private readonly IRelatedTaskMapper _relatedTaskMapper;

        public TaskDetailMapper(IRelatedTaskMapper relatedTaskMapper)
        {
            _relatedTaskMapper = relatedTaskMapper;
        }

        public TaskEntity Map(ITaskDetailModel model)
        {
            if (model == null)
                return new TaskEntity();
            return 
                new TaskEntity()
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
                RelatedTasks = model.RelatedTasks.Select(_relatedTaskMapper.Map).ToList()
            };
        }

        public ITaskDetailModel Map(TaskEntity item, int imagesCount)
        {
            if (item == null)
                return new TaskDetailModel();
            return new TaskDetailModel(item.Id, item.ActualDateTime, item.Name, imagesCount, item.Description,
                item.TaskDoneDateTime, item.TaskGroupId, item.StateId, item.Priority, item.WorkTime, item.RelatedTasks.Select(_relatedTaskMapper.Map).ToList());
        }

        //public Task<TaskEntity> MapAsync(ITaskDetailModel model, CancellationToken token)
        //{
        //    if (model == null)
        //        return Task.FromResult(new TaskEntity());
        //    return Task.FromResult(new TaskEntity
        //    {
        //        Id = model.Id,
        //        ActualDateTime = model.ActualDateTime,
        //        Name = model.Name,
        //        Description = model.Description,
        //        TaskDoneDateTime = model.TaskDoneDateTime,
        //        TaskGroupId = model.TaskGroupId,
        //        StateId = model.StateId,
        //        Priority = model.Priority.GetValue<EPriority>(),
        //        WorkTime = model.WorkTime,
        //    });
        //}

        //public async Task<ITaskDetailModel> MapAsync(TaskEntity entity, CancellationToken token)
        //{
        //    if (entity == null)
        //        return await Task.FromResult<ITaskDetailModel>(new TaskDetailModel());
        //    List<ITaskModel> relatedTasks = entity.RelatedTasks == null ? null :await _dbContext.TaskSet.AsQueryable().Where(s => entity.RelatedTasks.Any(x => x.Id == s.Id)).ToAsyncEnumerable().Select(_taskMapper.Map).ToListAsync(token);
        //    return await Task.FromResult<ITaskDetailModel>(new TaskDetailModel(entity.Id, entity.ActualDateTime, entity.Name, await _dbContext.ImageSet.AsQueryable().CountAsync(s => s.TaskId == entity.Id, token), entity.Description,
        //        entity.TaskDoneDateTime, entity.TaskGroupId, entity.StateId, entity.Priority, entity.WorkTime, relatedTasks));
        //}
    }
}