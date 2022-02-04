using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class CompanyFacade : FacadeBase<ICompanyModel, CompanyEntity>, ICompanyFacade
	{
		public new ICompanyRepository Repository { get; protected set; }

		public CompanyFacade(ICompanyRepository repository, IMapper<CompanyEntity, ICompanyModel> mapper) : base(repository, mapper)
		{
			Repository = repository;
		}

		public ICollection<ICompanyModel> GetCompaniesByUserId(Guid userId)
        {
            return Repository.GetWhere(s => s.UserId == userId).Select(Mapper.Map).ToList();
		}

        public async Task<ICollection<ICompanyModel>> GetCompaniesByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            return (await Repository.GetWhereAsync(s => s.UserId == userId, token)).Select(Mapper.Map).ToList();
        }

        public Task<bool> ExistsAsync(string companyName, CancellationToken token = default)
        {
            return Repository.ExistsAsync(s=>s.Name == companyName, token);
        }
    }
}