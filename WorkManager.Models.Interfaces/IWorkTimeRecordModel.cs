using System;

namespace WorkManager.Models.Interfaces
{
	public interface IWorkTimeRecordModel : IWorkRecordModelBase
	{
		TimeSpan WorkTime { get; set; }
		double PricePerHour { get; set; }
	}
}