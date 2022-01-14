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
        IEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDate(Guid companyId, EFilterType filterType);
        IAsyncEnumerable<IWorkRecordModelBase> GetAllRecordsByCompanyOrderedByDescendingDateAsync(Guid companyId, EFilterType filterType, CancellationToken token = default);
    }
}