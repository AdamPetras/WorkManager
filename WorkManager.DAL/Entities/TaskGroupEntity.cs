using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;

namespace WorkManager.DAL.Entities
{
	public class TaskGroupEntity : EntityBase
	{
		public TaskGroupEntity() : base()
		{
		}

		public string Name { get; set; }
		public string Description { get; set; }

		[Required]
		public Guid IdUser { get; set; }

		[ForeignKey(nameof(IdUser))]
		public virtual UserEntity User { get; set; }
	}
}