using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
	public class CompanyFacade : FacadeBase<ICompanyModel, CompanyEntity>, ICompanyFacade
    {
        protected new readonly ICompanyMapper Mapper;
        public CompanyFacade(WorkManagerDbContext workManagerDbContext, ICompanyMapper mapper,
            IDatabaseSessionController databaseSessionController) : base(workManagerDbContext, mapper, databaseSessionController)
        {
        }

		public ICollection<ICompanyModel> GetCompaniesByUserId(Guid userId)
        {
            DatabaseSessionController.Reset();
            return DbContext.CompanySet.AsQueryable().Where(s => s.UserId == userId).ToList().Select(s=> Mapper.Map(s, GetWorkRecordCount(s.Id))).ToList();
		}

        public async Task<ICollection<ICompanyModel>> GetCompaniesByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await (await DbContext.CompanySet.AsQueryable().Where(s => s.UserId == userId).ToListAsync(token)).ToAsyncEnumerable().SelectAwait(async s=> Mapper.Map(s, await GetWorkRecordCountAsync(s.Id, token))).ToListAsync(token);
        }

        public async Task<bool> ExistsAsync(string companyName, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await DbContext.CompanySet.AsQueryable().AnyAsync(s => s.Name == companyName, token);
        }

        public async Task RemoveAllByUserIdAsync(Guid userId, CancellationToken token = default)
        {
            using (IDbContextTransaction tx = await DbContext.Database.BeginTransactionAsync(token))
            {
                foreach (CompanyEntity entity in DbContext.CompanySet.AsQueryable().Where(s => s.UserId == userId))
                {
                    DbContext.Remove(entity);
                }
                await DbContext.SaveChangesAsync(token);
                await tx.CommitAsync(token);
            }
        }
        private int GetWorkRecordCount(Guid companyId) => DbContext.WorkSet.Count(s => s.CompanyId == companyId);
        private Task<int> GetWorkRecordCountAsync(Guid companyId, CancellationToken token) => DbContext.WorkSet.AsQueryable().CountAsync(s => s.CompanyId == companyId, token);
    }
}