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

		public bool Equals(ICompanyModel other)
		{
			return Equals((CompanyModel) other);
		}

		protected bool Equals(CompanyModel other)
		{
			return Name == other.Name && Equals(User, other.User);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((CompanyModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, User);
		}
	}
}