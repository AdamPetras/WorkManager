using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class WorkPiecesRecordModel:WorkRecordModelBase, IWorkPiecesRecordModel
	{
		public WorkPiecesRecordModel() : base(Guid.Empty, DateTime.Today, EWorkType.Both, Guid.Empty, string.Empty)
		{

		}

		public WorkPiecesRecordModel(Guid id, DateTime actualDateTime, uint pieces, double pricePerPiece, EWorkType type, string description, Guid companyId) : base(id, actualDateTime, type, companyId, description)
		{
			Pieces = pieces;
			PricePerPiece = pricePerPiece;
		}

		public uint Pieces { get; set; }
		public double PricePerPiece { get; set; }

		public bool Equals(IWorkPiecesRecordModel other)
		{
			return Equals((WorkPiecesRecordModel)other);
		}

		protected bool Equals(WorkPiecesRecordModel other)
		{
			return base.Equals(other) && Pieces == other.Pieces && PricePerPiece.Equals(other.PricePerPiece);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((WorkPiecesRecordModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(base.GetHashCode(), Pieces, PricePerPiece);
		}
	}
}