using WorkManager.Core.Exceptions;
using WorkManager.Core.Interfaces;

namespace WorkManager.Core
{
    public abstract class InitializedCheckerBase : IInitializedChecker
    {
        public bool Initialized { get; protected set; }
        public void CheckIsInitialized()
        {
            if (!Initialized)
                throw new NotInitializedException();
        }
    }
}