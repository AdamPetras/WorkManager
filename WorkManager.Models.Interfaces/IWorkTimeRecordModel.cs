using System;

namespace WorkManager.Models.Interfaces
{
	public interface IWorkTimeRecordModel : IWorkRecordModelBase, IEquatable<IWorkTimeRecordModel>
	{
		TimeSpan WorkTime { get; set; }
		double PricePerHour { get; set; }
	}
}