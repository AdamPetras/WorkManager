using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;

namespace WorkManager.BL.Providers
{
    public class ServerCurrentTimeProvider : IServerCurrentTimeProvider
    {
        private readonly WorkManagerDbContext _dbContext;

        public ServerCurrentTimeProvider(WorkManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DateTime GetTime()
        {
            IQueryable<ActualDateTimeEntity> dQuery = _dbContext.Set<ActualDateTimeEntity>().FromSqlRaw("SELECT \"04DC42DD-64C6-428A-94BE-F46390C1EF27\" AS Id, now() AS ActualDateTime");
            DateTime time = dQuery.First().ActualDateTime;
            return time;
        }

        public async Task<DateTime> GetTimeAsync(CancellationToken token)
        {
            IQueryable<ActualDateTimeEntity> dQuery = _dbContext.Set<ActualDateTimeEntity>().FromSqlRaw("SELECT \"04DC42DD-64C6-428A-94BE-F46390C1EF27\" AS Id, now() AS ActualDateTime");
            return (await dQuery.FirstAsync(token)).ActualDateTime;
        }
    }
}