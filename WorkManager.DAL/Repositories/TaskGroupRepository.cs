using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.DbContext.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.BaseClasses;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories
{
	public class TaskGroupRepository : RepositoryBase<TaskGroupEntity>, ITaskGroupRepository
	{
		public TaskGroupRepository(WorkManagerDbContext dbContext) : base(dbContext)
		{
		}

        protected override void AddInt(TaskGroupEntity entity, WorkManagerDbContext dbContext)
        {
            if (entity.User != null)
            {
                dbContext.UserSet.Attach(entity.User);
            }
        }
	}
}