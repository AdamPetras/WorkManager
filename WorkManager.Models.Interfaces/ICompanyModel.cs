using System;
using System.ComponentModel.DataAnnotations;
using WorkManager.DAL.Enums;

namespace WorkManager.Models.Interfaces
{
	public interface ICompanyModel : IModel, IEquatable<ICompanyModel>
	{
        [StringLength(30)]
		string Name { get; set; }
        int WorkRecordsCount { get; set; }
        Guid UserId { get; set; }
    }
}