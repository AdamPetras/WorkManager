using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class WorkBothRecordModel : WorkRecordModelBase, IWorkBothRecordModel
	{
		public WorkBothRecordModel() : base(Guid.Empty, DateTime.Today, EWorkType.Both, Guid.Empty, string.Empty)
		{

		}

		public WorkBothRecordModel(Guid id, DateTime actualDateTime, TimeSpan workTime, double pricePerHour, uint pieces, double pricePerPiece, EWorkType type, string description, Guid companyId):base(id, actualDateTime, type, companyId, description)
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

		protected bool Equals(WorkBothRecordModel other)
		{
			return base.Equals(other) && Pieces == other.Pieces && WorkTime.Equals(other.WorkTime) && PricePerPiece.Equals(other.PricePerPiece) && PricePerHour.Equals(other.PricePerHour);
		}

		public bool Equals(IWorkPiecesRecordModel other)
		{
			return Equals((WorkBothRecordModel) other);
		}

		public bool Equals(IWorkTimeRecordModel other)
		{
			return Equals((WorkBothRecordModel) other);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((WorkBothRecordModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(base.GetHashCode(), Pieces, WorkTime, PricePerPiece, PricePerHour);
		}
	}
}