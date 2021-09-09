using Unity;

namespace WorkManager.Extensions
{
	public static class ContainerExtension
	{
		public static void RegisterMultipleTypeSingleton<TI1, TI2, TClass>(this IUnityContainer containerRegistry) where TClass : TI1, TI2
		{
			containerRegistry.RegisterSingleton<TClass>();
			containerRegistry.RegisterSingleton<TI1, TClass>();
			containerRegistry.RegisterSingleton<TI2, TClass>();
		}

		public static void RegisterMultipleTypeSingleton<TI1, TI2, TI3, TClass>(this IUnityContainer containerRegistry) where TClass : TI1, TI2, TI3
		{
			RegisterMultipleTypeSingleton<TI1,TI2,TClass>(containerRegistry);
			containerRegistry.RegisterSingleton<TI3, TClass>();
		}

		public static void RegisterMultipleTypeSingleton<TI1, TI2, TI3, TI4, TClass>(this IUnityContainer containerRegistry) where TClass : TI1, TI2, TI3, TI4
		{
			RegisterMultipleTypeSingleton<TI1, TI2, TI3, TClass>(containerRegistry);
			containerRegistry.RegisterSingleton<TI4, TClass>();
		}
	}
}