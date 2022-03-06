using System;
using System.Threading.Tasks;
using WorkManager.Core.Interfaces;

namespace WorkManager.BL.Interfaces.Services
{
    public delegate Task AsyncEventHandler(object sender, EventArgs args);
    public interface IDatabaseSessionController : IInitializedChecker
    {
        AsyncEventHandler TimeoutExpiredAsyncEvent { get; set; }
        EventHandler TimeoutExpiredEvent { get; set; }
        TimeSpan CurrentTimeout { get; }
        void Reset();
        void Initialize(TimeSpan maximumTimeSpan);
    }
}