using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.Annotations;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class CompanyModel:ModelBase, ICompanyModel
	{
		public CompanyModel():base(Guid.Empty)
		{

		}

		public CompanyModel(Guid id, string name, Guid userId) : base(id)
		{
			Name = name;
            UserId = userId;
		}

        public CompanyModel([NotNull]ICompanyModel model) : this(model.Id,model.Name, model.UserId)
        {
            
        }

		public string Name { get; set; }
		public Guid UserId { get; set; }

		public bool Equals(ICompanyModel other)
		{
			return Equals((CompanyModel) other);
		}

		protected bool Equals(CompanyModel other)
		{
			return Name == other.Name && UserId == other.UserId;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((CompanyModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, UserId);
		}
	}
}