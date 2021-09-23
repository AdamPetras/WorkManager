using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace WorkManager.Converters
{
	public class VisibleIfAnyConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				throw new ArgumentException();
			if (value is IEnumerable enumerable)
			{
				if (enumerable.Cast<object>().Any())
				{
					return true;
				}
				return false;
			}
			throw new ArgumentException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}