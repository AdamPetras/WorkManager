using System;
using WorkManager.Core.Exceptions;
using Xamarin.Forms;

namespace WorkManager.Converters
{
	public class NegateBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is bool boolValue)
				return !boolValue;
			throw new InvalidTypeException(nameof(value), value.GetType());
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if(value is bool boolValue)
				return !boolValue;
			throw new InvalidTypeException(nameof(value), value.GetType());
		}
	}
}
