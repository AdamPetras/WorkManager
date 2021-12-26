using System;

namespace WorkManager.Extensions
{
	public static class DateTimeExtension
	{
		public static bool IsBetween(this DateTime input, DateTime from, DateTime to)
		{
			return (input.Date >= from.Date && input.Date <= to.Date);
		}
	}
}