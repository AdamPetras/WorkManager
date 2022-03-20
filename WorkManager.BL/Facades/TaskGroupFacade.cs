using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
    public class TaskGroupFacade : FacadeBase<ITaskGroupModel, TaskGroupEntity>, ITaskGroupFacade
    {
        protected new readonly ITaskGroupMapper Mapper;

        public TaskGroupFacade(WorkManagerDbContext dbContext, ITaskGroupMapper mapper,
            IDatabaseSessionController databaseSessionController) : base(dbContext, mapper, databaseSessionController)
        {
            Mapper = mapper;
        }

        public ICollection<ITaskGroupModel> GetTaskGroupsByUserId(Guid userId)
        {
            DatabaseSessionController.Reset();
            return DbContext.TaskGroupSet.AsQueryable().Where(s => s.UserId == userId).ToList().Select(s => Mapper.Map(s, GetTasksCount(s.Id))).ToList();
        }

        public async Task<ICollection<ITaskGroupModel>> GetTaskGroupsByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await (await DbContext.TaskGroupSet.AsQueryable().Where(s => s.UserId == userId).ToListAsync(token)).ToAsyncEnumerable().SelectAwait(async s => Mapper.Map(s, await GetTasksCountAsync(s.Id, token))).ToListAsync(token).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(string taskGroupName, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await DbContext.TaskGroupSet.AsQueryable().AnyAsync(s => s.Name == taskGroupName, token).ConfigureAwait(false);
        }

        public async Task RemoveAllByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            foreach (TaskGroupEntity entity in DbContext.TaskGroupSet.AsQueryable().Where(s => s.UserId == userId))
            {
                DbContext.Remove(entity);
            }
            await DbContext.SaveChangesAsync(token);
        }

        private int GetTasksCount(Guid taskGroupId) => DbContext.TaskSet.Count(s => s.TaskGroupId == taskGroupId);
        private Task<int> GetTasksCountAsync(Guid taskGroupId, CancellationToken token) => DbContext.TaskSet.AsQueryable().CountAsync(s => s.TaskGroupId == taskGroupId, token);
    }
}