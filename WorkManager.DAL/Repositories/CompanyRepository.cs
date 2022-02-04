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
    public class CompanyRepository : RepositoryBase<CompanyEntity>, ICompanyRepository
    {
        public CompanyRepository(WorkManagerDbContext dbContext) : base(dbContext)
        {
        }

        protected override void AddInt(CompanyEntity entity, WorkManagerDbContext dbContext)
        {
            if (entity.User != null)
            {
                dbContext.Entry(entity.User).State = EntityState.Unchanged;
            }
        }
    }
}