using System;
using System.Globalization;
using WorkManager.DAL.Enums;
using WorkManager.Xamarin.Core;
using Xamarin.Forms;

namespace WorkManager.Converters
{
	public class TaskPriorityToColorConverter:IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				throw new ArgumentException();
			if (value is LocalizedEnum priority)
			{
				return priority.GetValue<EPriority>() switch
				{
					EPriority.None => Color.Black,
					EPriority.VeryLow => Color.PaleGoldenrod,
					EPriority.Low => Color.Yellow,
					EPriority.Medium => Color.Orange,
					EPriority.High => Color.OrangeRed,
					EPriority.VeryHigh => Color.Red,
					_ => throw new ArgumentOutOfRangeException()
				};
			}
			throw new ArgumentException();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}