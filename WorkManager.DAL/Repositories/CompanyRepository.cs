﻿using System;
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
		public CompanyRepository(IDBContextFactory<WorkManagerDbContext> idbContextFactory) : base(idbContextFactory)
		{
		}

		protected override ICollection<CompanyEntity> GetAllInt(IQueryable<CompanyEntity> dbSet)
		{
			return dbSet.Include(s => s.User).ToList();
		}

		protected override void AddInt(CompanyEntity entity , WorkManagerDbContext dbContext)
		{
			if (entity.User != null)
			{
				dbContext.Entry(entity.User).State = EntityState.Unchanged;
			}
		}

		public ICollection<CompanyEntity> GetCompaniesByUserId(Guid userId)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return dbContext.CompanySet.Where(s => s.User.Id == userId).Include(s=>s.User).ToList();
			}
		}

        public async Task<ICollection<CompanyEntity>> GetCompaniesByUserIdAsync(Guid userId, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return await dbContext.CompanySet.Where(s => s.User.Id == userId).Include(s => s.User).ToListAsync(token);
            }
		}
    }
}