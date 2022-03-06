using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Facades.BaseClasses;
using WorkManager.BL.Interfaces;
using WorkManager.BL.Interfaces.Facades;
using WorkManager.BL.Interfaces.Services;
using WorkManager.DAL.Entities;
using WorkManager.DAL.Repositories.Interfaces;
using WorkManager.Models.Interfaces;

namespace WorkManager.BL.Facades
{
    public class UserFacade : FacadeBase<IUserModel, UserEntity>, IUserFacade
    {
        public new IUserRepository Repository { get; protected set; }
        public UserFacade(IUserRepository repository, IMapper<UserEntity, IUserModel> mapper,
            IDatabaseSessionController databaseSessionController) : base(repository, mapper, databaseSessionController)
        {
            Repository = repository;
        }

        public bool Exists(string username)
        {
            DatabaseSessionController.Reset();
            return Repository.Exists(s => s.Username == username);
        }

        public async Task<bool> ExistsAsync(string username, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await Repository.ExistsAsync(s => s.Username == username, token).ConfigureAwait(false);
        }

        public string GetPasswordByUserName(string username)
        {
            DatabaseSessionController.Reset();
            return Repository.GetPasswordByUserName(username);
        }

        public async Task<string> GetPasswordByUserNameAsync(string username, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await Repository.GetPasswordByUserNameAsync(username, token).ConfigureAwait(false);
        }

        public IUserModel GetByUserName(string username)
        {
            DatabaseSessionController.Reset();
            return Mapper.Map(Repository.GetByUserName(username));
        }

        public async Task<IUserModel> GetByUserNameAsync(string username, CancellationToken token = default)
        {
            DatabaseSessionController.Reset();
            return await Mapper.MapAsync(await Repository.GetByUserNameAsync(username, token).ConfigureAwait(false), token).ConfigureAwait(false);
        }
    }
}