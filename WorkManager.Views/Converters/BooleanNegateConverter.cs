using System;
using System.Globalization;
using Xamarin.Forms;

namespace WorkManager.Views.Converters
{
	public class BooleanNegateConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool val)
				return !val;
			throw new ArgumentException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}