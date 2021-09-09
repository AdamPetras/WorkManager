using System;

namespace WorkManager.Core
{
    public abstract class DisposableBase : IDisposable
    {
	    protected abstract void ReleaseUnmanagedResources();

	    protected virtual void ReleaseManagedResources()
	    {

	    }

	    private void Dispose(bool disposing)
	    {
		    ReleaseUnmanagedResources();
		    if (disposing)
		    {
			    ReleaseManagedResources();
		    }
	    }

	    public void Dispose()
	    {
		    Dispose(true);
		    GC.SuppressFinalize(this);
	    }

	    ~DisposableBase()
	    {
		    Dispose(false);
	    }
    }
}