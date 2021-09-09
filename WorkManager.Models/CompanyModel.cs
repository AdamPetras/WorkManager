using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class CompanyModel:ModelBase, ICompanyModel
	{
		public CompanyModel():base(Guid.Empty)
		{

		}

		public CompanyModel(Guid id, string name, IUserModel user) : base(id)
		{
			Name = name;
			User = user;
		}

		public string Name { get; set; }
		public IUserModel User { get; set; }
	}
}