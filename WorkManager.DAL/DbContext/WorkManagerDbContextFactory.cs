using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WorkManager.DAL.DbContext.Interfaces;

namespace WorkManager.DAL.DbContext
{
	public class WorkManagerDbContextFactory: Interfaces.IDBContextFactory<WorkManagerDbContext>
	{
		private readonly DatabaseVersionChecker _databaseVersionChecker;

		public WorkManagerDbContextFactory(DatabaseVersionChecker databaseVersionChecker)
		{
			_databaseVersionChecker = databaseVersionChecker;
		}

		public WorkManagerDbContext CreateDbContext()
		{
			WorkManagerDbContext postDatabaseContext = (WorkManagerDbContext)Activator.CreateInstance(typeof(WorkManagerDbContext));
			if (postDatabaseContext == null)
				throw new TypeLoadException(nameof(WorkManagerDbContext));
			if (postDatabaseContext.Database.EnsureCreated())
			{
				//zápis verze do databáze aby bylo možné aplikaci spustit
				_databaseVersionChecker.CreateTableVersionIfNotExists(postDatabaseContext.DatabasePath);
				_databaseVersionChecker.WriteLatestVersion(postDatabaseContext.DatabasePath);
			}
			if(postDatabaseContext.Database.GetAppliedMigrations() == null)
				postDatabaseContext.Database.Migrate();
			return postDatabaseContext;
		}

        public async Task<WorkManagerDbContext> CreateDbContextAsync(CancellationToken token)
        {
            WorkManagerDbContext postDatabaseContext = (WorkManagerDbContext)Activator.CreateInstance(typeof(WorkManagerDbContext));
            if (postDatabaseContext == null)
                throw new TypeLoadException(nameof(WorkManagerDbContext));
            if (await postDatabaseContext.Database.EnsureCreatedAsync(token))
            {
                //zápis verze do databáze aby bylo možné aplikaci spustit
                _databaseVersionChecker.CreateTableVersionIfNotExists(postDatabaseContext.DatabasePath);
                _databaseVersionChecker.WriteLatestVersion(postDatabaseContext.DatabasePath);
            }
            if (await postDatabaseContext.Database.GetAppliedMigrationsAsync(token) == null)
                await postDatabaseContext.Database.MigrateAsync(token);
            return postDatabaseContext;
        }
	}
}