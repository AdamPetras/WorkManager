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
        ICollection<ICompanyModel> GetCompaniesByUserId(Guid userId);
        Task<ICollection<ICompanyModel>> GetCompaniesByUserIdAsync(Guid userId, CancellationToken token = default);
        Task<bool> ExistsAsync(string companyName, CancellationToken token = default);
        Task RemoveAllByUserIdAsync(Guid userId, CancellationToken token = default);
    }
}