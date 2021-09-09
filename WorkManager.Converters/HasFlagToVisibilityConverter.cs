using System;
using System.Globalization;
using Xamarin.Forms;

namespace WorkManager.Converters
{
	public class HasFlagToVisibilityConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Enum enumValue && parameter is Enum enumParameter)
			{
				if (enumValue.GetType() == enumParameter.GetType())
				{
					return enumValue.HasFlag(enumParameter);
				}
			}
			throw new ArgumentException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}