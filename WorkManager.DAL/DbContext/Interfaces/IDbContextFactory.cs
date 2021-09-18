namespace WorkManager.DAL.DbContext.Interfaces
{
	public interface IDBContextFactory<TDbContext> where TDbContext : Microsoft.EntityFrameworkCore.DbContext
	{
		TDbContext CreateDbContext();
	}
}