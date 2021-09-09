using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace WorkManager.DAL.DbContext
{
	public class WorkManagerDbContextFactory: Interfaces.IDbContextFactory<WorkManagerDbContext>
	{
		public WorkManagerDbContext CreateDbContext()
		{
			WorkManagerDbContext postDatabaseContext = (WorkManagerDbContext)Activator.CreateInstance(typeof(WorkManagerDbContext));
			if (postDatabaseContext == null)
				throw new TypeLoadException(nameof(WorkManagerDbContext));
			postDatabaseContext.Database.EnsureCreated();
			if(postDatabaseContext.Database.GetAppliedMigrations() == null)
				postDatabaseContext.Database.Migrate();
			return postDatabaseContext;
		}
	}
}