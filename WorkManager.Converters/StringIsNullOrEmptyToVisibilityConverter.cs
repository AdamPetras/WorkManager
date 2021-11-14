using System;
using System.Globalization;
using Xamarin.Forms;

namespace WorkManager.Converters
{
    public class StringIsNullOrEmptyToVisibilityConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                null => false,
                string s when string.IsNullOrWhiteSpace(s) => false,
                _ => true
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}