using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class WorkBothRecordModel : WorkRecordModelBase, IWorkBothRecordModel
	{
		public WorkBothRecordModel() : base(Guid.Empty, DateTime.Today, EWorkType.Both, null, string.Empty)
		{

		}

		public WorkBothRecordModel(Guid id, DateTime actualDateTime, TimeSpan workTime, double pricePerHour, uint pieces, double pricePerPiece, EWorkType type, string description, ICompanyModel company):base(id, actualDateTime, type, company, description)
		{
			WorkTime = workTime;
			PricePerHour = pricePerHour;
			Pieces = pieces;
			PricePerPiece = pricePerPiece;
		}

		public uint Pieces { get; set; }
		public TimeSpan WorkTime { get; set; }
		public double PricePerPiece { get; set; }
		public double PricePerHour { get; set; }
	}
}