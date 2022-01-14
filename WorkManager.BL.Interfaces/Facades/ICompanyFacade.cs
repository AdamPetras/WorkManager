using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;
namespace WorkManager.BL.Interfaces.Facades
{
	public interface ICompanyFacade : IFacade<ICompanyModel>
	{
        IEnumerable<ICompanyModel> GetCompaniesByUserId(Guid userId);
        IAsyncEnumerable<ICompanyModel> GetCompaniesByUserIdAsync(Guid userId, CancellationToken token = default);
        Task<bool> ExistsAsync(string companyName, CancellationToken token = default);
    }
}