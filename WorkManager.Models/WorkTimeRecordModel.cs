using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class WorkTimeRecordModel : WorkRecordModelBase, IWorkTimeRecordModel
	{
		public WorkTimeRecordModel():base(Guid.Empty, DateTime.Today, EWorkType.Both, null, string.Empty)
		{
			
		}

		public WorkTimeRecordModel(Guid id, DateTime actualDateTime, TimeSpan workTime, double pricePerHour, EWorkType type, string description, ICompanyModel company) : base(id, actualDateTime, type, company, description)
		{
			WorkTime = workTime;
			PricePerHour = pricePerHour;
		}

		public TimeSpan WorkTime { get; set; }
		public double PricePerHour { get; set; }
	}
}