using System;
using System.Linq.Expressions;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WorkManager.BL.Interfaces.Services;
using Xamarin.Forms;

namespace WorkManager.BL.Services
{
	public class SettingsService : ISettingsServiceManager
	{
		public T GetValue<T>([CallerMemberName] string propertyName = null)
		{
			if (propertyName == null)
				throw new ArgumentException();
			if (Application.Current.Properties.ContainsKey(propertyName))
			{
				return (T)Application.Current.Properties[propertyName];
			}
			return default;
		}

		public async Task SetValue<T>(T value, [CallerMemberName] string propertyName = null)
		{
			Application.Current.Properties[propertyName] = value;
			await Application.Current.SavePropertiesAsync();
		}

		public async Task SetValue<T>(Expression<Func<T>> expr)
		{
			MemberExpression body = (MemberExpression)expr.Body;
			Application.Current.Properties[body.Member.Name] = expr.Compile().DynamicInvoke();
			await Application.Current.SavePropertiesAsync();
		}
	}
}