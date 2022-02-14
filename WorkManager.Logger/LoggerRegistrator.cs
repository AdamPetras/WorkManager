using System.Linq;
using System.Reflection;
using Unity;
using WorkManager.BL.Interfaces.Providers;

namespace WorkManager.Logger
{
    public class LoggerRegistrator
    {
        private readonly IUnityContainer _unityContainer;

        public LoggerRegistrator(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public void Register()
        {
            MethodInfo factoryMethod = typeof(LoggerRegistrator).GetMethods(BindingFlags.Static | BindingFlags.Public).First(x => x.ContainsGenericParameters);

            _unityContainer.RegisterFactory(typeof(ILogger<>),
                (container, type, _) =>
                {
                    MethodInfo genFactoryMethod = factoryMethod.MakeGenericMethod(type.GetGenericArguments()[0]);
                    return genFactoryMethod.Invoke(null, new object[] { container.Resolve<IServerCurrentTimeProvider>() });
                });
        }

        public static ILogger<T> Factory<T>(IServerCurrentTimeProvider serverCurrentTimeProvider)
        {
            return new AppCenterLogger<T>(serverCurrentTimeProvider);
        }
    }
}