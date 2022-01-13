using System;
using WorkManager.DAL.Enums;

namespace WorkManager.Models.Interfaces
{
	public interface ICompanyModel : IModel, IEquatable<ICompanyModel>
	{
		string Name { get; set; }
        Guid UserId { get; set; }
	}
}