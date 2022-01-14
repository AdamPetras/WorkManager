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

		public IEnumerable<ICompanyModel> GetCompaniesByUserId(Guid userId)
		{
			return Repository.GetCompaniesByUserId(userId).Select(Mapper.Map);
		}

        public async IAsyncEnumerable<ICompanyModel> GetCompaniesByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            foreach (CompanyEntity entity in await Repository.GetCompaniesByUserIdAsync(userId, token))
            {
                yield return await Mapper.MapAsync(entity, token);
            }
        }

        public Task<bool> ExistsAsync(string companyName, CancellationToken token = default)
        {
            return Repository.ExistsAsync(companyName, token);
        }
    }
}