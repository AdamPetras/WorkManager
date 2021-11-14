using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IWorkRecordRepository : IRepository<WorkRecordEntity>
	{
        ICollection<WorkRecordEntity> GetAllRecordsByCompany(Guid companyId, EFilterType filterType);
        Task<ICollection<WorkRecordEntity>> GetAllRecordsByCompanyAsync(Guid companyId, EFilterType filterType, CancellationToken token);
    }
}