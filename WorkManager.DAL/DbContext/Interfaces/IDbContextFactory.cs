using System.Threading;
using System.Threading.Tasks;

namespace WorkManager.DAL.DbContext.Interfaces
{
	public interface IDBContextFactory<TDbContext> where TDbContext : Microsoft.EntityFrameworkCore.DbContext
	{
		TDbContext CreateDbContext();
    }
}