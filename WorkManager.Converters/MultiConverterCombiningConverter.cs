using System;
using Xamarin.Forms;

namespace WorkManager.Converters
{
	public class MultiConverterCombiningConverter : IValueConverter
	{
		public IValueConverter[] ConverterList { get; set; }
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (ConverterList == null || ConverterList.Length == 0)
			{
				return null;
			}
			object convertedValue = ConverterList[0].Convert(value, targetType, parameter, culture);
			for (int index = 1; index < ConverterList.Length; index++)
			{
				convertedValue = ConverterList[index].Convert(convertedValue, targetType, parameter, culture);
			}
			return convertedValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}