using System;
using WorkManager.DAL.Enums;

namespace WorkManager.Models.Interfaces
{
	public interface IWorkRecordModelFactory
	{
		IWorkRecordModelBase CreateWorkRecord(Guid id, DateTime actualDateTime, TimeSpan workTime, double pricePerHour,
			uint pieces, double pricePerPiece, EWorkType type, string description, ICompanyModel company);
		IWorkRecordModelBase CopyRecord(IWorkRecordModelBase model);
	}
}