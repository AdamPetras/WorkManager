using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Entities
{
	public class TaskGroupEntity : IEntity
	{
		public TaskGroupEntity() : base()
		{
		}
        [Key]
        public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public virtual UserEntity User { get; set; }
	}
}