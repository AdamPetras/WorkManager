using System;
using WorkManager.DAL.Enums;
using WorkManager.Models.Annotations;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class CompanyModel:ModelBase, ICompanyModel
	{
        private uint _workRecordsCount;

        public CompanyModel():base(Guid.Empty)
		{

		}

		public CompanyModel(Guid id, string name, uint workRecordsCount, Guid userId) : base(id)
		{
			Name = name;
            WorkRecordsCount = workRecordsCount;
            UserId = userId;
		}

        public CompanyModel([NotNull]ICompanyModel model) : this(model.Id,model.Name, model.WorkRecordsCount, model.UserId)
        {
            
        }

		public string Name { get; set; }
        public uint WorkRecordsCount
        {
            get => _workRecordsCount;
            set
            {
                _workRecordsCount = value;
				RaisePropertyChanged();
            }
        }
		public Guid UserId { get; set; }

        public bool Equals(ICompanyModel other)
		{
			return Equals((CompanyModel) other);
		}

		protected bool Equals(CompanyModel other)
		{
			return Name == other.Name && UserId == other.UserId && WorkRecordsCount == other.WorkRecordsCount;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((CompanyModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, UserId, WorkRecordsCount);
		}
	}
}