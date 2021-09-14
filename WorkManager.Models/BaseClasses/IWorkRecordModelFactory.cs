using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models.BaseClasses
{
	public interface IWorkRecordModelFactory
	{
		IWorkRecordModelBase CreateWorkRecord(Guid id, DateTime actualDateTime, TimeSpan workTime, double pricePerHour,
			uint pieces, double pricePerPiece, EWorkType type, string description, ICompanyModel company);
		IWorkRecordModelBase CopyRecord(IWorkRecordModelBase model);
	}
}