﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.BaseClasses;
using WorkManager.DAL.DbContext.Interfaces;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories
{
	public class UserRepository:RepositoryBase<UserEntity>, IUserRepository
	{
		public UserRepository(IDBContextFactory<WorkManagerDbContext> idbContextFactory) : base(idbContextFactory)
		{

		}

		public UserEntity GetByUserName(string username)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
				return dbContext.UserSet.FirstOrDefault(s => s.Username == username);
		}

		public async Task<UserEntity> GetByUserNameAsync(string username, CancellationToken token)
		{
			using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
				return await dbContext.UserSet.FirstOrDefaultAsync(s => s.Username == username, cancellationToken: token);
		}

		public string GetPasswordByUserName(string username)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
				return dbContext.UserSet.FirstOrDefault(s => s.Username == username)?.Password;
		}

		public async Task<string> GetPasswordByUserNameAsync(string username, CancellationToken token)
		{
			using (WorkManagerDbContext dbContext = await IdbContextFactory.CreateDbContextAsync(token))
				return (await dbContext.UserSet.FirstOrDefaultAsync(s => s.Username == username, token))?.Password;
		}

		public bool Exists(string username)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
				return dbContext.UserSet.Any(s => s.Username == username);
		}

		public async Task<bool> ExistsAsync(string username, CancellationToken token)
		{
			using (WorkManagerDbContext dbContext = IdbContextFactory.CreateDbContext())
				return await dbContext.UserSet.AnyAsync(s => s.Username == username, token);
		}

		protected override ICollection<UserEntity> GetAllInt(IQueryable<UserEntity> dbSet)
		{
			return dbSet.ToList();
		}

		protected override void AddInt(UserEntity entity, WorkManagerDbContext dbContext)
		{
			//není žádná implementace protože není potřeba nastavovat changed context
			return;
		}
	}
}