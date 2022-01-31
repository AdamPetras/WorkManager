using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Enums;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Interfaces.Facades
{
	public interface IWorkRecordFacade: IFacade<IWorkRecordModelBase>
	{
        IEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId);
        IAsyncEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, CancellationToken token = default);
        IAsyncEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyAsync(Guid companyId, CancellationToken token = default);
        IAsyncEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(Guid companyId, DateTime from, DateTime to, CancellationToken token = default);

        Task<double> GetPriceTotalThisMonthAsync(Guid companyId, DateTime today, CancellationToken token = default);
        Task<double> GetPriceTotalThisYearAsync(Guid companyId, DateTime today, CancellationToken token = default);
    }
}