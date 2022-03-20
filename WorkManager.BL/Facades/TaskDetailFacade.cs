using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
    public class TaskDetailFacade : FacadeBase<ITaskDetailModel, TaskEntity>, ITaskDetailFacade
    {
        protected new readonly ITaskDetailMapper Mapper;

        public TaskDetailFacade(WorkManagerDbContext dbContext, ITaskDetailMapper mapper, IDatabaseSessionController databaseSessionController) : base(dbContext, mapper, databaseSessionController)
        {
            Mapper = mapper;
        }

        public async Task<ITaskDetailModel> GetByIdAsync(Guid taskId, CancellationToken token)
        {
            DatabaseSessionController.Reset();
            TaskEntity entity = await DbContext.TaskSet.Include(s => s.RelatedTasks).SingleOrDefaultAsync(s => s.Id == taskId, token);
            return Mapper.Map(entity, await GetImagesCountAsync(entity.Id,token));
        }

        private int GetImagesCount(Guid taskId) => DbContext.ImageSet.Count(s => s.TaskId == taskId);
        private Task<int> GetImagesCountAsync(Guid taskId, CancellationToken token) => DbContext.ImageSet.AsQueryable().CountAsync(s => s.TaskId == taskId, token);
    }
}