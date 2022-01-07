using System;
using System.Globalization;
using Xamarin.Forms;

namespace WorkManager.Converters
{
    public class VisibleIfEqualConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            if (value.Equals(parameter))
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
	}
}