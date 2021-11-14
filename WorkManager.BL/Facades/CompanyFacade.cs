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

        public async Task<IEnumerable<ICompanyModel>> GetCompaniesByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            return (await Repository.GetCompaniesByUserIdAsync(userId, token)).Select(Mapper.Map);
        }
    }
}