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
			return Repository.GetTaskGroupsByUserId(userId).Select(Mapper.Map).ToList();
		}
	}
}