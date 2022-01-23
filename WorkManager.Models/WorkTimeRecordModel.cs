using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;
using WorkManager.Xamarin.Core;

namespace WorkManager.Models
{
	public class WorkTimeRecordModel : WorkRecordModelBase, IWorkTimeRecordModel
	{
		public WorkTimeRecordModel():base(Guid.Empty, DateTime.Today, EWorkType.Both, Guid.Empty, string.Empty)
		{
			
		}

		public WorkTimeRecordModel(Guid id, DateTime actualDateTime, TimeSpan workTime, double pricePerHour, EWorkType type, string description, Guid companyId) : base(id, actualDateTime, type, companyId, description)
		{
			WorkTime = workTime;
			PricePerHour = pricePerHour;
		}

		public TimeSpan WorkTime { get; set; }
		public double PricePerHour { get; set; }

		public bool Equals(IWorkTimeRecordModel other)
		{
			return Equals((WorkTimeRecordModel)other);
		}

		protected bool Equals(WorkTimeRecordModel other)
		{
			return base.Equals(other) && WorkTime.Equals(other.WorkTime) && PricePerHour.Equals(other.PricePerHour);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((WorkTimeRecordModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(base.GetHashCode(), WorkTime, PricePerHour);
		}
	}
}