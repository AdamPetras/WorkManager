using System.Runtime.CompilerServices;

namespace WorkManager.BL.Interfaces.Services
{
	public interface ISettingsServiceProvider
	{
		T GetValue<T>([CallerMemberName] string propertyName = null);
	}
}