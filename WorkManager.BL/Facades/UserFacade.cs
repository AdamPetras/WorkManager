using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Mappers;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.DbContext;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
    public class UserFacade : FacadeBase<IUserModel, UserEntity>, IUserFacade
    {
        protected new readonly IUserMapper Mapper;

        public UserFacade(WorkManagerDbContext dbContext, IUserMapper mapper,
            IDatabaseSessionController databaseSessionController) : base(dbContext, mapper, databaseSessionController)
        {
            Mapper = mapper;
        }

        public bool Exists(string username)
        {
            DatabaseSessionController.Reset();
            return DbContext.UserSet.Any(s => s.Username == username);
        }

        public async Task<bool> ExistsAsync(string username, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            var values = await DbContext.UserSet.ToListAsync(token);
            return await DbContext.UserSet.AnyAsync(s => s.Username == username, token).ConfigureAwait(false);
        }

        public string GetPasswordByUserName(string username)
        {
            DatabaseSessionController.Reset();
            return DbContext.UserSet.FirstOrDefault(s => s.Username == username)?.Password;
        }

        public async Task<string> GetPasswordByUserNameAsync(string username, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return (await DbContext.UserSet.SingleOrDefaultAsync(s => s.Username == username, token))?.Password;
        }

        public IUserModel GetByUserName(string username)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(DbContext.UserSet.AsQueryable().SingleOrDefault(s => s.Username == username));
        }

        public async Task<IUserModel> GetByUserNameAsync(string username, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(await DbContext.UserSet.SingleOrDefaultAsync(s => s.Username == username, token));
        }
    }
}