using System;
using System.Threading;
using System.Threading.Tasks;
using WorkManager.Core.Exceptions;

namespace WorkManager.Core
{
    public abstract class LoopingBase : InitializedCheckerBase
    {
	    private TimeSpan _start;
	    private TimeSpan _interval;
        private Thread _loopThread;

        protected LoopingBase(TimeSpan start, TimeSpan interval)
        {
            Initialize(start, interval);
        }

        protected LoopingBase(TimeSpan interval) : this(TimeSpan.Zero, interval)
        {
        }

        protected LoopingBase()
        {
        }

        protected void Initialize(TimeSpan start, TimeSpan interval)
        {
            if(Initialized)
                throw new AlreadyInitializedException();
            _start = start;
            _interval = interval;
            State = ELoopingState.New;
            Initialized = true;
        }

        protected ELoopingState State { get; private set; }

        protected void StartLoop()
        {
            CheckIsInitialized();
            _loopThread = new Thread(LoopThread);
            _loopThread.Start();
        }

	    protected void StopLoop()
        {
            CheckIsInitialized();
            State = ELoopingState.Stopped;
            _loopThread.Join();  //počkám až vlákno umře
            State = ELoopingState.Terminated;
        }

        protected void RestartLoop()
        {
            CheckIsInitialized();
            if(State == ELoopingState.Running)
                StopLoop();
            if(State != ELoopingState.Running)
                StartLoop();
        }

        private void LoopThread()
        {
            CheckIsInitialized();
            State = ELoopingState.Running;
            Thread.Sleep(_start);
            while (State == ELoopingState.Running)
            {
                Loop();
                Thread.Sleep(_interval);
            }
        }

        protected abstract void Loop();
    }
}