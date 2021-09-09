using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models.BaseClasses
{
	public class WorkRecordModelFactory : IWorkRecordModelFactory
	{
		public IWorkRecordModelBase CreateWorkRecord(Guid id, DateTime actualDateTime, TimeSpan workTime, double pricePerHour,
			uint pieces, double pricePerPiece, EWorkType type, string description, ICompanyModel company)
		{
			return type switch
			{
				EWorkType.Time => new WorkTimeRecordModel(id, actualDateTime, workTime, pricePerHour, type, description, company),
				EWorkType.Piece => new WorkPiecesRecordModel(id, actualDateTime, pieces, pricePerPiece, type, description, company),
				EWorkType.Both => new WorkBothRecordModel(id, actualDateTime, workTime, pricePerHour, pieces, pricePerPiece,type, description, company),
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}