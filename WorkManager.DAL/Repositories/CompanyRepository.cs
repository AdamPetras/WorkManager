using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.BaseClasses;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories
{
	public class CompanyRepository : RepositoryBase<CompanyEntity>, ICompanyRepository
	{
		public CompanyRepository(DbContext.Interfaces.IDbContextFactory<WorkManagerDbContext> dbContextFactory) : base(dbContextFactory)
		{
		}

		protected override IEnumerable<CompanyEntity> GetAllInt(IQueryable<CompanyEntity> dbSet)
		{
			return dbSet.Include(s => s.User).ToList();
		}

		protected override void AddInt(CompanyEntity entity , WorkManagerDbContext dbContext)
		{
			if (entity.User != null)
			{
				dbContext.Entry(entity.User).State = EntityState.Unchanged;
			}
		}

		public ICollection<CompanyEntity> GetCompaniesByUserId(Guid userId)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
			{
				return dbContext.CompanySet.Where(s => s.User.Id == userId).Include(s=>s.User).ToList();
			}
		}
	}
}