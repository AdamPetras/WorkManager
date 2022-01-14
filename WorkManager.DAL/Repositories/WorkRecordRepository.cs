using System;
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
			return dbSet.ToList();
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

		public ICollection<WorkRecordEntity> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId, EFilterType filterType)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
			{
				return filterType switch
				{
					EFilterType.None => dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).OrderByDescending(s => s.ActualDateTime).ToList(),
					EFilterType.ThisYear => dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).Where(s => s.ActualDateTime.Year == DateTime.Today.Year).OrderByDescending(s => s.ActualDateTime).ToList(),
					EFilterType.ThisMonth => dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).Where(s => s.ActualDateTime.Month == DateTime.Today.Month && s.ActualDateTime.Year == DateTime.Today.Year).OrderByDescending(s => s.ActualDateTime).ToList(),
					_ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null)
				};
			}
		}

        public async Task<ICollection<WorkRecordEntity>> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, EFilterType filterType, CancellationToken token)
        {
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return filterType switch
                {
                    EFilterType.None => await dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).OrderByDescending(s=>s.ActualDateTime).ToListAsync(token),
                    EFilterType.ThisYear => await dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).Where(s => s.ActualDateTime.Year == DateTime.Today.Year).OrderByDescending(s => s.ActualDateTime).ToListAsync(token),
                    EFilterType.ThisMonth => await dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).Where(s => s.ActualDateTime.Month == DateTime.Today.Month && s.ActualDateTime.Year == DateTime.Today.Year).OrderByDescending(s => s.ActualDateTime).ToListAsync(token),
                    _ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null)
                };
            }
		}

        public uint GetRecordCountInCompany(Guid companyId)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return (uint)dbContext.WorkSet.AsQueryable().Count(s => s.CompanyId == companyId);
			}
        }

        public async Task<uint> GetRecordCountInCompanyAsync(Guid companyId, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return (uint)await dbContext.WorkSet.AsQueryable().CountAsync(s => s.CompanyId == companyId, token);
            }
        }
    }
}