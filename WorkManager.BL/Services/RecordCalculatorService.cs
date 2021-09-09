using System;
using WorkManager.BL.Interfaces.Services;
using WorkManager.Models.Interfaces;
using WorkManager.Models.Interfaces.ModelServices;

namespace WorkManager.BL.Services
{
	public class RecordCalculatorService: IRecordCalculatorService
	{
		public double Calculate<T>(T record) where T : IWorkRecordModelBase
		{
			return record switch
			{
				IWorkBothRecordModel bothRecord => (bothRecord.Pieces * bothRecord.PricePerPiece) +
				                                   (bothRecord.WorkTime.TotalHours * bothRecord.PricePerHour),
				IWorkTimeRecordModel timeRecord => timeRecord.WorkTime.TotalHours * timeRecord.PricePerHour,
				IWorkPiecesRecordModel piecesRecord => piecesRecord.Pieces * piecesRecord.PricePerPiece,
				_ => throw new ArgumentException()
			};
		}
	}
}