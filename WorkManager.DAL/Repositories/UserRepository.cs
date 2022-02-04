using System.Collections.Generic;
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
		public UserRepository(WorkManagerDbContext dbContext) : base(dbContext)
		{

		}

		public UserEntity GetByUserName(string username)
		{
            return DbContext.UserSet.FirstOrDefault(s => s.Username == username);
		}

		public async Task<UserEntity> GetByUserNameAsync(string username, CancellationToken token)
		{
            return await DbContext.UserSet.AsQueryable().FirstOrDefaultAsync(s => s.Username == username, token);
		}

		public string GetPasswordByUserName(string username)
		{
            return DbContext.UserSet.FirstOrDefault(s => s.Username == username)?.Password;
		}

		public async Task<string> GetPasswordByUserNameAsync(string username, CancellationToken token)
		{
            return (await DbContext.UserSet.AsQueryable().FirstOrDefaultAsync(s => s.Username == username, token))?.Password;
		}

		protected override void AddInt(UserEntity entity, WorkManagerDbContext dbContext)
		{
			//není žádná implementace protože není potřeba nastavovat changed context
			return;
		}
	}
}