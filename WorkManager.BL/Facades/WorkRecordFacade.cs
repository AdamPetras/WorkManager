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
	public class WorkRecordFacade : FacadeBase<IWorkRecordModelBase, WorkRecordEntity>, IWorkRecordFacade
	{
		protected new IWorkRecordRepository Repository;

		public WorkRecordFacade(IWorkRecordRepository repository, IMapper<WorkRecordEntity, IWorkRecordModelBase> mapper) : base(repository, mapper)
		{
			Repository = repository;
		}

		public ICollection<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId)
		{
			return Repository.GetWhereOrderByDescending(s=>s.CompanyId == companyId,s=>s.ActualDateTime).Select(Mapper.Map).ToList();
		}

        public async Task<ICollection<IWorkRecordModelBase>> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, CancellationToken token = default)
        {
			return (await Repository.GetWhereOrderByDescendingAsync(s=>s.CompanyId == companyId,s=>s.ActualDateTime, token)).Select(Mapper.Map).ToList();
        }

        public async Task<ICollection<IWorkRecordModelBase>> GetAllRecordsByCompanyAsync(Guid companyId, CancellationToken token = default)
        {
			return (await Repository.GetWhereAsync(s=>s.CompanyId == companyId, token)).Select(Mapper.Map).ToList();
        }

        public async Task<ICollection<IWorkRecordModelBase>> GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(Guid companyId, DateTime from, DateTime to,
            CancellationToken token = default)
        {
			return (await Repository.GetWhereOrderByDescendingAsync(s => s.CompanyId == companyId && s.ActualDateTime.Date >= from.Date &&
                                                                         s.ActualDateTime.Date <= to.Date, s=>s.ActualDateTime, token)).Select(Mapper.Map).ToList();
        }

        public Task<double> GetPriceTotalThisMonthAsync(Guid companyId, DateTime today, CancellationToken token = default)
        {
            return Repository.GetPriceTotalThisMonthAsync(companyId, today, token);
        }

        public Task<double> GetPriceTotalThisYearAsync(Guid companyId, DateTime today, CancellationToken token = default)
        {
            return Repository.GetPriceTotalThisYearAsync(companyId, today, token);
        }
    }
}