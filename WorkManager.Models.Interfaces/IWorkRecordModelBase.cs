using System;
using WorkManager.DAL.Enums;

namespace WorkManager.Models.Interfaces
{
	public interface IWorkRecordModelBase : IModel
	{
		DateTime ActualDateTime { get; set; }
		EWorkType Type { get; set; }
		ICompanyModel Company { get; set; }
		string Description { get; set; }
		double CalculatedPrice { get; }
	}
}