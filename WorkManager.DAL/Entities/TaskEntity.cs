using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;

namespace WorkManager.DAL.Entities
{
	public class TaskEntity : EntityBase
	{
		public TaskEntity()
		{
		}

		public DateTime ActualDateTime { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime TaskDoneDateTime { get; set; }

		[Required]
		public Guid IdState { get; set; }

		[ForeignKey(nameof(IdState))]
		public virtual KanbanStateEntity State { get; set; }

		[Required]
		public Guid IdTaskGroup { get; set; }

		[ForeignKey(nameof(IdTaskGroup))]
		public virtual TaskGroupEntity TaskGroup { get; set; }
	}
}