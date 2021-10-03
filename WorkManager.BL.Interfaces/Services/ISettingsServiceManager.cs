using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WorkManager.BL.Interfaces.Services
{
	public interface ISettingsServiceManager : ISettingsServiceProvider
	{
		Task SetValue<T>(T value, [CallerMemberName] string propertyName = null);
		Task SetValue<T>(Expression<Func<T>> expr);
	}
}