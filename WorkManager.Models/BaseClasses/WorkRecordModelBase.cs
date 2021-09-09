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

		protected WorkRecordModelBase(Guid id, DateTime actualDateTime, EWorkType type, ICompanyModel company, string description) : base(id)
		{
			ActualDateTime = actualDateTime;
			Type = type;
			Company = company;
			Description = description;
			_recordCalculatorService = ServiceLocator.Current.GetInstance<IRecordCalculatorService>();
		}

		public DateTime ActualDateTime { get; set; }
		public EWorkType Type { get; set; }
		public ICompanyModel Company { get; set; }
		public string Description { get; set; }
		public double CalculatedPrice => _recordCalculatorService.Calculate(this);
	}
}