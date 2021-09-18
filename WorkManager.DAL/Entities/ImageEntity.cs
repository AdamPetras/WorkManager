using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;

namespace WorkManager.DAL.Entities
{
	public class ImageEntity : EntityBase
	{
		public string Path { get; set; }
		public string Description { get; set; }
		[Required]
		public Guid TaskId { get; set; }
		[ForeignKey(nameof(TaskId))]
		public virtual TaskEntity Task { get; set; }
	}
}