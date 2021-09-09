using System;
using System.Collections;
using System.Collections.Generic;
using WorkManager.Core.Exceptions;
using Xamarin.Forms;

namespace WorkManager.Converters
{
	public class ListIsEmptyToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is IList lstValue)
				return lstValue.Count == 0;
			throw new InvalidTypeException(nameof(value), value.GetType());
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}