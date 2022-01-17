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

        public async IAsyncEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, CancellationToken token = default)
        {
            foreach (WorkRecordEntity workRecordEntity in await Repository.GetAllRecordsByCompanyOrderedByDescendingDateAsync(companyId,token))
            {
                yield return await Mapper.MapAsync(workRecordEntity, token);
            }
		}

        public async IAsyncEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(Guid companyId, DateTime from, DateTime to,
            CancellationToken token = default)
        {
            foreach (WorkRecordEntity workRecordEntity in await Repository.GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(companyId,from,to, token))
            {
                yield return await Mapper.MapAsync(workRecordEntity, token);
            }
        }
    }
}