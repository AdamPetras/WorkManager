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
        ICollection<WorkRecordEntity> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId);
        Task<ICollection<WorkRecordEntity>> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, CancellationToken token);
        uint GetRecordCountInCompany(Guid companyId);
        Task<uint> GetRecordCountInCompanyAsync(Guid companyId, CancellationToken token);
        Task<IEnumerable<WorkRecordEntity>> GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(Guid companyId, DateTime from, DateTime to, CancellationToken token);
    }
}