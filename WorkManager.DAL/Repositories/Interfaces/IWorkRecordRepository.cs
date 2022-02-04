using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Enums;

namespace WorkManager.DAL.Repositories.Interfaces
{
	public interface IWorkRecordRepository : IRepository<WorkRecordEntity>
	{
        Task<double> GetPriceTotalThisMonthAsync(Guid companyId, DateTime today, CancellationToken token);
        Task<double> GetPriceTotalThisYearAsync(Guid companyId, DateTime today, CancellationToken token);
    }
}