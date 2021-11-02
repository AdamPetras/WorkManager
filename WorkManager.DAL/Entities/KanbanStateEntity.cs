using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;

namespace WorkManager.DAL.Entities
{
	public class KanbanStateEntity : EntityBase
	{
		public string Name { get; set; }
		public int StateOrder { get; set; }
		public string IconName { get; set; }
		[Required]
		public Guid IdTaskGroup { get; set; }

		[ForeignKey(nameof(IdTaskGroup))]
		public virtual TaskGroupEntity TaskGroup { get; set; }
	}
}