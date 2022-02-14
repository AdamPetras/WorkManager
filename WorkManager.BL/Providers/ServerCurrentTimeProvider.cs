using System;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces.Providers;
using WorkManager.DAL.Repositories;

namespace WorkManager.BL.Providers
{
    public class ServerCurrentTimeProvider : IServerCurrentTimeProvider
    {
        private readonly SystemRepository _systemRepository;

        public ServerCurrentTimeProvider(SystemRepository systemRepository)
        {
            _systemRepository = systemRepository;
        }

        public DateTime GetTime()
        {
            return _systemRepository.ActualServerTime();
        }

        public Task<DateTime> GetTimeAsync(CancellationToken token)
        {
            return _systemRepository.ActualServerTimeAsync(token);
        }
    }
}