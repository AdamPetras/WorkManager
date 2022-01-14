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
        ICollection<WorkRecordEntity> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId, EFilterType filterType);
        Task<ICollection<WorkRecordEntity>> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, EFilterType filterType, CancellationToken token);
        uint GetRecordCountInCompany(Guid companyId);
        Task<uint> GetRecordCountInCompanyAsync(Guid companyId, CancellationToken token);
    }
}