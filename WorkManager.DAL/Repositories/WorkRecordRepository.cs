﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.DbContext.Interfaces;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;
using WorkManager.DAL.Repositories.BaseClasses;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories
{
	public class WorkRecordRepository : RepositoryBase<WorkRecordEntity>, IWorkRecordRepository
	{
		public WorkRecordRepository(IDBContextFactory<WorkManagerDbContext> idbContextFactory) : base(idbContextFactory)
		{
		}

		protected override ICollection<WorkRecordEntity> GetAllInt(IQueryable<WorkRecordEntity> dbSet)
		{
			return dbSet.Include(s => s.Company).ToList();
		}

		protected override void AddInt(WorkRecordEntity entity, WorkManagerDbContext dbContext)
		{
			if (entity.Company != null)
			{
				dbContext.Entry(entity.Company).State = EntityState.Unchanged;
				if (entity.Company.User != null)
					dbContext.Entry(entity.Company.User).State = EntityState.Unchanged;
			}
		}

		public ICollection<WorkRecordEntity> GetAllRecordsByCompany(Guid companyId, EFilterType filterType)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return filterType switch
				{
					EFilterType.None => dbContext.WorkSet.Where(s => s.Company.Id == companyId).Include(s=>s.Company).ThenInclude(s=>s.User).ToList(),
					EFilterType.ThisYear => dbContext.WorkSet.Where(s => s.Company.Id == companyId)
						.Where(s => s.ActualDateTime.Year == DateTime.Today.Year).Include(s => s.Company).ThenInclude(s => s.User)
						.ToList(),
					EFilterType.ThisMonth => dbContext.WorkSet.Where(s => s.Company.Id == companyId).Include(s => s.Company)
						.Where(s => s.ActualDateTime.Month == DateTime.Today.Month &&
						            s.ActualDateTime.Year == DateTime.Today.Year).Include(s => s.Company).ThenInclude(s => s.User)
						.ToList(),
					_ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null)
				};
			}
		}

        public async Task<ICollection<WorkRecordEntity>> GetAllRecordsByCompanyAsync(Guid companyId, EFilterType filterType, CancellationToken token)
        {
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return filterType switch
                {
                    EFilterType.None => await dbContext.WorkSet.Where(s => s.Company.Id == companyId).Include(s => s.Company).ThenInclude(s => s.User).ToListAsync(token),
                    EFilterType.ThisYear => await dbContext.WorkSet.Where(s => s.Company.Id == companyId)
                        .Where(s => s.ActualDateTime.Year == DateTime.Today.Year).Include(s => s.Company).ThenInclude(s => s.User)
                        .ToListAsync(token),
                    EFilterType.ThisMonth => await dbContext.WorkSet.Where(s => s.Company.Id == companyId).Include(s => s.Company)
                        .Where(s => s.ActualDateTime.Month == DateTime.Today.Month &&
                                    s.ActualDateTime.Year == DateTime.Today.Year).Include(s => s.Company).ThenInclude(s => s.User)
                        .ToListAsync(token),
                    _ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null)
                };
            }
		}
    }
}