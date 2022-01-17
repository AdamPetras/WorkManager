using System;
using WorkManager.DAL.Enums;
using WorkManager.Xamarin.Core;

namespace WorkManager.Models.Interfaces
{
	public interface IWorkRecordModelBase : IModel, IEquatable<IWorkRecordModelBase>
	{
		DateTime ActualDateTime { get; set; }
        LocalizedEnum Type { get; set; }
		Guid CompanyId { get; set; }
		string Description { get; set; }
		double CalculatedPrice { get; }
	}
}