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
        IEnumerable<IWorkRecordModelBase> GetAllRecordsByCompany(Guid companyId, EFilterType filterType);
        Task<IEnumerable<IWorkRecordModelBase>> GetAllRecordsByCompanyAsync(Guid companyId, EFilterType filterType, CancellationToken token = default);
    }
}