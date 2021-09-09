using Microsoft.EntityFrameworkCore;

namespace WorkManager.DAL.Interfaces
{
	public interface IDbContextFactory<TDbContext> where TDbContext : DbContext
	{
		TDbContext CreateDbContext();
	}
}