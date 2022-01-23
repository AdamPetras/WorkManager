using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.Interfaces;
using WorkManager.Xamarin.Core;

namespace WorkManager.Models.BaseClasses
{
	public class WorkRecordModelFactory : IWorkRecordModelFactory
	{
		public IWorkRecordModelBase CreateWorkRecord(Guid id, DateTime actualDateTime, TimeSpan workTime, double pricePerHour,
			uint pieces, double pricePerPiece, EWorkType type, string description, Guid companyId)
		{
			return type switch
			{
				EWorkType.Time => new WorkTimeRecordModel(id, actualDateTime, workTime, pricePerHour, type, description, companyId),
				EWorkType.Piece => new WorkPiecesRecordModel(id, actualDateTime, pieces, pricePerPiece, type, description, companyId),
				EWorkType.Both => new WorkBothRecordModel(id, actualDateTime, workTime, pricePerHour, pieces, pricePerPiece,type, description, companyId),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		public IWorkRecordModelBase CopyRecord(IWorkRecordModelBase model)
		{
			return model.Type switch
			{
				EWorkType.Time => new WorkTimeRecordModel(((IWorkTimeRecordModel)model).Id, ((IWorkTimeRecordModel)model).ActualDateTime, ((IWorkTimeRecordModel)model).WorkTime, ((IWorkTimeRecordModel)model).PricePerHour,
					((IWorkTimeRecordModel)model).Type, ((IWorkTimeRecordModel)model).Description, ((IWorkTimeRecordModel)model).CompanyId),
				EWorkType.Piece => new WorkPiecesRecordModel(((IWorkPiecesRecordModel)model).Id, ((IWorkPiecesRecordModel)model).ActualDateTime, ((IWorkPiecesRecordModel)model).Pieces, 
					((IWorkPiecesRecordModel)model).PricePerPiece, ((IWorkPiecesRecordModel)model).Type, ((IWorkPiecesRecordModel)model).Description, ((IWorkPiecesRecordModel)model).CompanyId),
				EWorkType.Both => new WorkBothRecordModel(((IWorkBothRecordModel)model).Id, ((IWorkBothRecordModel)model).ActualDateTime, ((IWorkBothRecordModel)model).WorkTime, ((IWorkBothRecordModel)model).PricePerHour,
					((IWorkBothRecordModel)model).Pieces, ((IWorkBothRecordModel)model).PricePerPiece, ((IWorkBothRecordModel)model).Type, ((IWorkBothRecordModel)model).Description, ((IWorkBothRecordModel)model).CompanyId),
				_ => throw new ArgumentOutOfRangeException()
			};
		}
	}
}