using System;
using System.ComponentModel.DataAnnotations;
using WorkManager.DAL.Enums;
using WorkManager.Xamarin.Core;

namespace WorkManager.Models.Interfaces
{
	public interface IWorkRecordModelBase : IModel, IEquatable<IWorkRecordModelBase>
	{
		DateTime ActualDateTime { get; set; }
        EWorkType Type { get; set; }
		Guid CompanyId { get; set; }
        [StringLength(300)]
		string Description { get; set; }
		double CalculatedPrice { get; }
	}
}