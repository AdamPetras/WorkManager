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
        public WorkRecordRepository(WorkManagerDbContext dbContext) : base(dbContext)
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

        public async Task<double> GetPriceTotalThisMonthAsync(Guid companyId, DateTime today, CancellationToken token)
        {
            return await DbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).Where(s=>s.ActualDateTime.Year == today.Year && s.ActualDateTime.Month == today.Month).AsAsyncEnumerable().SumAsync(Calculate, token).ConfigureAwait(false);
        }

        public async Task<double> GetPriceTotalThisYearAsync(Guid companyId, DateTime today, CancellationToken token)
        {
            return await DbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).Where(s => s.ActualDateTime.Year == today.Year).AsAsyncEnumerable().SumAsync(Calculate,token).ConfigureAwait(false);
        }

        private double Calculate(WorkRecordEntity record)
        {
            return (record.Pieces * record.PricePerPiece) + (record.WorkTime.TotalHours * record.PricePerHour);
        }
    }
}