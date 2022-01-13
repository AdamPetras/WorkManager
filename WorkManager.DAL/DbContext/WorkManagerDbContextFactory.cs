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

		public WorkManagerDbContextFactory()
		{
		}

		public WorkManagerDbContext CreateDbContext()
		{
			WorkManagerDbContext postDatabaseContext = (WorkManagerDbContext)Activator.CreateInstance(typeof(WorkManagerDbContext));
			if (postDatabaseContext == null)
				throw new TypeLoadException(nameof(WorkManagerDbContext));
            postDatabaseContext.Database.EnsureCreated();
			return postDatabaseContext;
		}

        public async Task<WorkManagerDbContext> CreateDbContextAsync(CancellationToken token)
        {
            WorkManagerDbContext postDatabaseContext = (WorkManagerDbContext)Activator.CreateInstance(typeof(WorkManagerDbContext));
            if (postDatabaseContext == null)
                throw new TypeLoadException(nameof(WorkManagerDbContext));
            postDatabaseContext.Database.EnsureCreated();
            return postDatabaseContext;
        }
	}
}