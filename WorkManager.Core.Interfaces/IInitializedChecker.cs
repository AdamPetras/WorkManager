namespace WorkManager.Core.Interfaces
{
    public interface IInitializedChecker
    {
        bool Initialized { get; }
        void CheckIsInitialized();
    }
}