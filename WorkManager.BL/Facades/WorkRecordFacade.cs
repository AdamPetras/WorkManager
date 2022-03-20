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
	public class WorkRecordFacade : FacadeBase<IWorkRecordModelBase, WorkRecordEntity>, IWorkRecordFacade
	{
        protected new readonly IWorkRecordMapper Mapper;

        public WorkRecordFacade(WorkManagerDbContext dbContext,
            IWorkRecordMapper mapper,
            IDatabaseSessionController databaseSessionController) : base(dbContext, mapper, databaseSessionController)
        {
            Mapper = mapper;
        }

		public ICollection<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId)
		{
            DatabaseSessionController.Reset();
			return DbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).OrderByDescending(s => s.ActualDateTime).Select(Mapper.Map).ToList();
		}

        public async Task<ICollection<IWorkRecordModelBase>> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			return await DbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).OrderByDescending(s => s.ActualDateTime).ToAsyncEnumerable().Select(Mapper.Map).ToListAsync(token);
        }

        public async Task<ICollection<IWorkRecordModelBase>> GetAllRecordsByCompanyAsync(Guid companyId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			return await DbContext.WorkSet.AsQueryable().Where(s=>s.CompanyId == companyId).ToAsyncEnumerable().Select(Mapper.Map).ToListAsync(token);
        }

        public async Task<ICollection<IWorkRecordModelBase>> GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(Guid companyId, DateTime from, DateTime to,
            CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
			return await DbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId && s.ActualDateTime.Date >= from.Date && s.ActualDateTime.Date <= to.Date)
                .OrderByDescending(s => s.ActualDateTime).ToAsyncEnumerable().Select(Mapper.Map).ToListAsync(token);
        }

        public async Task<double> GetPriceTotalThisMonthAsync(Guid companyId, DateTime today, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await DbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId)
                .Where(s => s.ActualDateTime.Year == today.Year && s.ActualDateTime.Month == today.Month)
                .ToAsyncEnumerable().SumAsync(Calculate, token).ConfigureAwait(false);
        }

        public async Task<double> GetPriceTotalThisYearAsync(Guid companyId, DateTime today, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await DbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).Where(s => s.ActualDateTime.Year == today.Year).AsAsyncEnumerable().SumAsync(Calculate, token).ConfigureAwait(false);
        }

        public async Task RemoveAllByCompanyIdAsync(Guid companyId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            foreach (WorkRecordEntity entity in DbContext.WorkSet.AsQueryable().Where(s => s.CompanyId == companyId).AsEnumerable())
            {
                DbContext.Remove(entity);
            }
            await DbContext.SaveChangesAsync(token);
        }

        private double Calculate(WorkRecordEntity record)
        {
            return (record.Pieces * record.PricePerPiece) + (record.WorkTime.TotalHours * record.PricePerHour);
        }
    }
}