using System;
using System.Collections.Generic;
using WorkManager.DAL.Entities;
using WorkManager.Models.Interfaces;
namespace WorkManager.BL.Interfaces.Facades
{
	public interface ICompanyFacade : IFacade<ICompanyModel>
	{
		ICollection<ICompanyModel> GetCompaniesByUserId(Guid userId);
	}
}