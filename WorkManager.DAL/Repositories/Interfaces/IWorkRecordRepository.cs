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
        IAsyncEnumerable<WorkRecordEntity> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, CancellationToken token);
        uint GetRecordCountInCompany(Guid companyId);
        Task<uint> GetRecordCountInCompanyAsync(Guid companyId, CancellationToken token);
        IAsyncEnumerable<WorkRecordEntity> GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(Guid companyId, DateTime from, DateTime to, CancellationToken token);
        IAsyncEnumerable<WorkRecordEntity> GetAllRecordsByCompanyAsync(Guid companyId, CancellationToken token);
        Task<double> GetPriceTotalThisMonthAsync(Guid companyId, DateTime today, CancellationToken token);
        Task<double> GetPriceTotalThisYearAsync(Guid companyId, DateTime today, CancellationToken token);
    }
}