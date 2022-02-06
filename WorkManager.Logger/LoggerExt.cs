using WorkManager.DAL.Repositories;

namespace WorkManager.Logger
{
    public class LoggerExt
    {
        public static ILogger<T> CreateLogger<T>(SystemRepository repository)
        {
            return new AppCenterLogger<T>(repository);
        }
    }
}