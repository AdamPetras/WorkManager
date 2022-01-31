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
    public class WorkRecordRepository : RepositoryBase<WorkRecordEntity>, IWorkRecordRepository
    {
        public WorkRecordRepository(IDBContextFactory<WorkManagerDbContext> idbContextFactory) : base(idbContextFactory)
        {
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

        public ICollection<WorkRecordEntity> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId)
                    .OrderByDescending(s => s.ActualDateTime).ToList();
            }
        }

        public IAsyncEnumerable<WorkRecordEntity> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId)
                             .OrderByDescending(s => s.ActualDateTime).ToList().ToAsyncEnumerable();
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

        public IAsyncEnumerable<WorkRecordEntity> GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(Guid companyId, DateTime from, DateTime to,
            CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId && s.ActualDateTime.Date >= from.Date &&
                             s.ActualDateTime.Date <= to.Date).OrderByDescending(s => s.ActualDateTime).ToList().ToAsyncEnumerable();
            }
        }

        public IAsyncEnumerable<WorkRecordEntity> GetAllRecordsByCompanyAsync(Guid companyId, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).ToList().ToAsyncEnumerable();
            }
        }

        public async Task<double> GetPriceTotalThisMonthAsync(Guid companyId, DateTime today, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return await dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).Where(s=>s.ActualDateTime.Year == today.Year && s.ActualDateTime.Month == today.Month).AsAsyncEnumerable().SumAsync(Calculate, token);
            }
        }

        public async Task<double> GetPriceTotalThisYearAsync(Guid companyId, DateTime today, CancellationToken token)
        {
            using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
            {
                return await dbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).Where(s => s.ActualDateTime.Year == today.Year).AsAsyncEnumerable().SumAsync(Calculate,token);
            }
        }

        private double Calculate(WorkRecordEntity record)
        {
            return (record.Pieces * record.PricePerPiece) + (record.WorkTime.TotalHours * record.PricePerHour);
        }
    }
}