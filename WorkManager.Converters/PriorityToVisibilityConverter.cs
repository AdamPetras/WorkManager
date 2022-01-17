using System;
using System.Globalization;
using WorkManager.DAL.Enums;
using WorkManager.Xamarin.Core;
using Xamarin.Forms;

namespace WorkManager.Converters
{
	public class PriorityToVisibilityConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				throw new ArgumentException();
			if (value is LocalizedEnum priority)
			{
				if (priority.GetValue<EPriority>() == EPriority.None)
					return false;
				return true;
			}
			throw new ArgumentException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}