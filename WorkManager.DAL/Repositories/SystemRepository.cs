using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;

namespace WorkManager.DAL.Repositories
{
    public class SystemRepository
    {
        private readonly WorkManagerDbContext _dbContext;

        public SystemRepository(WorkManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DateTime ActualServerTime()
        {
            IQueryable<ActualDateTimeEntity> dQuery = _dbContext.Set<ActualDateTimeEntity>().FromSqlRaw("SELECT \"04DC42DD-64C6-428A-94BE-F46390C1EF27\" AS Id, now() AS ActualDateTime");
            DateTime time = dQuery.First().ActualDateTime;
            return time;
        }
    }
}