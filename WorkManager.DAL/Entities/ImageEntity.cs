using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Entities
{
	public class ImageEntity : IEntity
	{
        [Key]
        public Guid Id { get; set; }
        [StringLength(200)]
		public string Path { get; set; }
        [StringLength(300)]
		public string Description { get; set; }
        [Required]
		public Guid TaskId { get; set; }
		[ForeignKey(nameof(TaskId))]
		public virtual TaskEntity Task { get; set; }
	}
}