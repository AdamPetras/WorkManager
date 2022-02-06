using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class TaskGroupFacade : FacadeBase<ITaskGroupModel,TaskGroupEntity>, ITaskGroupFacade
	{
		public new ITaskGroupRepository Repository { get; }

		public TaskGroupFacade(ITaskGroupRepository repository, IMapper<TaskGroupEntity, ITaskGroupModel> mapper) : base(repository, mapper)
		{
			Repository = repository;
		}

		public ICollection<ITaskGroupModel> GetTaskGroupsByUserId(Guid userId)
		{
			return Repository.GetWhere(s=>s.UserId == userId).Select(Mapper.Map).ToList();
		}

        public async Task<ICollection<ITaskGroupModel>> GetTaskGroupsByUserIdAsync(Guid userId, CancellationToken token = default)
        {
			return (await Repository.GetWhereAsync(s=> s.UserId == userId, token).ConfigureAwait(false)).Select(Mapper.Map).ToList();
        }

        public async Task<bool> ExistsAsync(string taskGroupName, CancellationToken token = default)
        {
            return await Repository.ExistsAsync(s=>s.Name == taskGroupName, token).ConfigureAwait(false);
        }
    }
}