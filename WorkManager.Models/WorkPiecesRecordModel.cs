using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class WorkPiecesRecordModel:WorkRecordModelBase, IWorkPiecesRecordModel
	{
		public WorkPiecesRecordModel() : base(Guid.Empty, DateTime.Today, EWorkType.Both, null, string.Empty)
		{

		}

		public WorkPiecesRecordModel(Guid id, DateTime actualDateTime, uint pieces, double pricePerPiece, EWorkType type, string description, ICompanyModel company) : base(id, actualDateTime, type, company, description)
		{
			Pieces = pieces;
			PricePerPiece = pricePerPiece;
		}

		public uint Pieces { get; set; }
		public double PricePerPiece { get; set; }
	}
}