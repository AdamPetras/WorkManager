using System;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Core;
using WorkManager.Core.Exceptions;
using Xamarin.Essentials.Interfaces;

namespace WorkManager.BL.Services
{
    public class DatabaseSessionController : LoopingBase, IDatabaseSessionController
    {
        private readonly IMainThread _mainThread;
        private TimeSpan _maxTimeout;

        public DatabaseSessionController(IMainThread mainThread)
        {
            _mainThread = mainThread;
        }

        public AsyncEventHandler TimeoutExpiredAsyncEvent { get; set; }
        public EventHandler TimeoutExpiredEvent { get; set; }
        public TimeSpan CurrentTimeout { get; private set; }
        public new bool Initialized { get; protected set; } //předchozí třída obsahuje bool, zda je inicializováno, zde je to samé

        private readonly object _criticSection = new();


       
        public void Reset()
        {
            CheckIsInitialized();
            lock (_criticSection)
            {
                CurrentTimeout = _maxTimeout;
            }
            if(State == ELoopingState.New)
                StartLoop();
            if(State is ELoopingState.Stopped or ELoopingState.Terminated)
                RestartLoop();
        }

        public void Initialize(TimeSpan maximumTimeSpan)
        {
            if (Initialized)
                throw new AlreadyInitializedException();
            Initialize(TimeSpan.Zero,TimeSpan.FromSeconds(maximumTimeSpan.TotalSeconds/5));
            lock (_criticSection)
            {
                _maxTimeout = maximumTimeSpan;
            }
            Initialized = true;
            Reset();
        }

        private void OnTimeoutExpiredEvent()
        {
            TimeoutExpiredEvent?.Invoke(this, null);
        }

        private async Task OnTimeoutExpiredAsyncEvent()
        {
            if (TimeoutExpiredAsyncEvent != null)
                await TimeoutExpiredAsyncEvent?.Invoke(this, null);
        }

        protected override void Loop()
        {
            lock (_criticSection)
            {
                CurrentTimeout -= TimeSpan.FromSeconds(_maxTimeout.TotalSeconds / 5);
            }
            if (CurrentTimeout <= TimeSpan.Zero)
            {
                _mainThread.BeginInvokeOnMainThread(OnTimeoutExpiredEvent);
                _mainThread.InvokeOnMainThreadAsync(OnTimeoutExpiredAsyncEvent);
                //Volám z hlavního vlákna, protože je ve vnitř thread.Join a v podstatě by čekalo vlákno samo na sebe
                _mainThread.BeginInvokeOnMainThread(StopLoop);
            }
        }

        public new void CheckIsInitialized() //předchozí třída obsahuje tuto metodu...
        {
            if (!Initialized)
                throw new NotInitializedException();
        }
    }
}