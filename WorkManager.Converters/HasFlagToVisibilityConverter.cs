using System;
using System.Globalization;
using WorkManager.Xamarin.Core;
using Xamarin.Forms;

namespace WorkManager.Converters
{
	public class HasFlagToVisibilityConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is LocalizedEnum enumValue && parameter is Enum enumParameter)	// TODO LOCALIZED ENUM!!!!!!
			{
				bool val = enumValue.GetValue(parameter.GetType()).HasFlag(enumParameter);
                return val;
            }
			throw new ArgumentException(@"Hodnota value není localized Enum");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}