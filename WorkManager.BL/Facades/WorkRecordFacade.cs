using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;
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

		public IEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId)
		{
			return Repository.GetAllRecordsByCompanyOrderedByDescendingDate(companyId).Select(Mapper.Map);
		}

        public IAsyncEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, CancellationToken token = default)
        {
            return Repository.GetAllRecordsByCompanyOrderedByDescendingDateAsync(companyId,token).SelectAwait(async workRecordEntity => await Mapper.MapAsync(workRecordEntity, token));
        }

        public IAsyncEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyAsync(Guid companyId, CancellationToken token = default)
        {
            return Repository.GetAllRecordsByCompanyAsync(companyId, token).SelectAwait(async workRecordEntity => await Mapper.MapAsync(workRecordEntity, token));
        }

        public IAsyncEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(Guid companyId, DateTime from, DateTime to,
            CancellationToken token = default)
        {
            return Repository.GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(companyId, from,to, token).SelectAwait(async workRecordEntity => await Mapper.MapAsync(workRecordEntity, token));
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