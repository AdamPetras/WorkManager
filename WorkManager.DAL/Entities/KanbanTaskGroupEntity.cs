using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;

namespace WorkManager.DAL.Entities
{
	public class KanbanTaskGroupEntity:EntityBase
	{
		[Required]
		public Guid IdTaskGroup { get; set; }

		[ForeignKey(nameof(IdTaskGroup))]
		public virtual TaskGroupEntity TaskGroup { get; set; }

		[Required]
		public Guid IdKanban { get; set; }

		[ForeignKey(nameof(IdKanban))]
		public virtual KanbanStateEntity Kanban { get; set; }
	}
}