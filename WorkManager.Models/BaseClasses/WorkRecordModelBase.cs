using System;
using CommonServiceLocator;
using WorkManager.DAL.Enums;
using WorkManager.Models.Interfaces;
using WorkManager.Models.Interfaces.ModelServices;

namespace WorkManager.Models.BaseClasses
{
	public abstract class WorkRecordModelBase : ModelBase, IWorkRecordModelBase
	{
		private readonly IRecordCalculatorService _recordCalculatorService;

		protected WorkRecordModelBase(Guid id, DateTime actualDateTime, EWorkType type, Guid companyId, string description) : base(id)
		{
			ActualDateTime = actualDateTime;
			Type = type;
            CompanyId = companyId;
			Description = description;
			_recordCalculatorService = ServiceLocator.Current.GetInstance<IRecordCalculatorService>();
		}

		public DateTime ActualDateTime { get; set; }
		public EWorkType Type { get; set; }
		public Guid CompanyId { get; set; }
		public string Description { get; set; }
		public double CalculatedPrice => _recordCalculatorService.Calculate(this);

		public bool Equals(IWorkRecordModelBase other)
		{
			return Equals((object)other);
		}

		protected bool Equals(WorkRecordModelBase other)
		{
			return ActualDateTime.Equals(other.ActualDateTime) && Type == other.Type && CompanyId == other.CompanyId && Description == other.Description;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((WorkRecordModelBase) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ActualDateTime, (int) Type, CompanyId, Description);
		}
	}
}