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

		public IEnumerable<IWorkRecordModelBase> GetAllRecordsByCompany(Guid companyId, EFilterType filterType)
		{
			return Repository.GetAllRecordsByCompany(companyId, filterType).Select(Mapper.Map);
		}

        public async Task<IEnumerable<IWorkRecordModelBase>> GetAllRecordsByCompanyAsync(Guid companyId, EFilterType filterType, CancellationToken token = default)
        {
			return (await Repository.GetAllRecordsByCompanyAsync(companyId, filterType, token)).Select(Mapper.Map);
		}
	}
}