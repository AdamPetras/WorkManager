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
        ICollection<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId);
        Task<ICollection<IWorkRecordModelBase>> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, CancellationToken token = default);
        Task<ICollection<IWorkRecordModelBase>> GetAllRecordsByCompanyAsync(Guid companyId, CancellationToken token = default);
        Task<ICollection<IWorkRecordModelBase>> GetAllRecordsByCompanyOrderedByDescendingDateFromToAsync(Guid companyId, DateTime from, DateTime to, CancellationToken token = default);

        Task<double> GetPriceTotalThisMonthAsync(Guid companyId, DateTime today, CancellationToken token = default);
        Task<double> GetPriceTotalThisYearAsync(Guid companyId, DateTime today, CancellationToken token = default);
        Task RemoveAllByCompanyIdAsync(Guid companyId, CancellationToken token = default);
    }
}