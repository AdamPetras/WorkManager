using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.BaseClasses;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.Repositories.Interfaces;

namespace WorkManager.DAL.Repositories
{
	public class UserRepository:RepositoryBase<UserEntity>, IUserRepository
	{
		public UserRepository(DbContext.Interfaces.IDbContextFactory<WorkManagerDbContext> dbContextFactory) : base(dbContextFactory)
		{

		}

		public UserEntity GetByUserName(string username)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
				return dbContext.UserSet.FirstOrDefault(s => s.Username == username);
		}

		public async Task<UserEntity> GetByUserNameAsync(string username)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
				return await dbContext.UserSet.FirstOrDefaultAsync(s => s.Username == username);
		}

		public string GetPasswordByUserName(string username)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
				return dbContext.UserSet.FirstOrDefault(s => s.Username == username)?.Password;
		}

		public async Task<string> GetPasswordByUserNameAsync(string username)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
				return (await dbContext.UserSet.FirstOrDefaultAsync(s => s.Username == username))?.Password;
		}

		public bool Exists(string username)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
				return dbContext.UserSet.Any(s => s.Username == username);
		}

		public async Task<bool> ExistsAsync(string username)
		{
			using (WorkManagerDbContext dbContext = DbContextFactory.CreateDbContext())
				return await dbContext.UserSet.AnyAsync(s => s.Username == username);
		}

		protected override IEnumerable<UserEntity> GetAllInt(IQueryable<UserEntity> dbSet)
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