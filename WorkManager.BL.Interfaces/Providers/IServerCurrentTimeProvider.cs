using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkManager.BL.Interfaces.Providers
{
    public interface IServerCurrentTimeProvider
    {
        DateTime GetTime();
        Task<DateTime> GetTimeAsync(CancellationToken token);
    }
}