using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Enums;

namespace WorkManager.DAL.Entities
{
	public class WorkRecordEntity : EntityBase
	{
		public WorkRecordEntity()
		{
		}
		public DateTime ActualDateTime { get; set; }

		[Required]
		public Guid IdCompany { get; set; }

		[ForeignKey(nameof(IdCompany))]
		public virtual CompanyEntity Company { get; set; }
		public EWorkType Type { get; set; }
		public TimeSpan WorkTime { get; set; }
		public double PricePerHour { get; set; }
		public uint Pieces { get; set; }
		public double PricePerPiece { get; set; }
		public string Description { get; set; }
	}
}