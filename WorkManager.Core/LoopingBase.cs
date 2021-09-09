using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkManager.Core
{
    public abstract class LoopingBase : DisposableBase
    {
	    private readonly TimeSpan _start;
	    private readonly TimeSpan _interval;
	    private readonly Task _task;
	    private readonly CancellationTokenSource _tokenSource;

	    protected LoopingBase(TimeSpan start, TimeSpan interval)
	    {
		    _start = start;
		    _interval = interval;
		    _tokenSource = new CancellationTokenSource();
		    _task = Task.Factory.StartNew(LoopThread, _tokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
	    }

	    private void LoopThread()
	    {
		    _task.Wait(_start);
		    while (!_tokenSource.IsCancellationRequested)
		    {
			    Loop();
			    _task.Wait(_interval);
		    }
	    }

	    protected void Start()
	    {
		    _task.Start();
	    }

	    protected void Stop()
	    {
		    _tokenSource.Cancel();
		    ReleaseUnmanagedResources();
	    }

	    protected abstract void Loop();

	    protected override void ReleaseUnmanagedResources()
	    {
		    _task.Dispose();
	    }
    }
}