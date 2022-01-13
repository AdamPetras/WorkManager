using System;
using WorkManager.DAL.Enums;

namespace WorkManager.Models.Interfaces
{
	public interface IWorkRecordModelBase : IModel, IEquatable<IWorkRecordModelBase>
	{
		DateTime ActualDateTime { get; set; }
		EWorkType Type { get; set; }
		Guid CompanyId { get; set; }
		string Description { get; set; }
		double CalculatedPrice { get; }
	}
}