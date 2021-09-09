namespace WorkManager.DAL.DbContext.Interfaces
{
	public interface IDbContextFactory<TDbContext> where TDbContext : Microsoft.EntityFrameworkCore.DbContext
	{
		TDbContext CreateDbContext();
	}
}