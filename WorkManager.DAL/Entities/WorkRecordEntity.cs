using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.DAL.Enums;

namespace WorkManager.DAL.Entities
{
	public class WorkRecordEntity : IEntity
	{
		public WorkRecordEntity()
		{
		}
        [Key]
        public Guid Id { get; set; }
		public DateTime ActualDateTime { get; set; }

		[Required]
		public Guid CompanyId { get; set; }

		[ForeignKey(nameof(CompanyId))]
		public virtual CompanyEntity Company { get; set; }
		public EWorkType Type { get; set; }
		public TimeSpan WorkTime { get; set; }
		public double PricePerHour { get; set; }
		public uint Pieces { get; set; }
		public double PricePerPiece { get; set; }
        [StringLength(300)]
		public string Description { get; set; }
	}
}