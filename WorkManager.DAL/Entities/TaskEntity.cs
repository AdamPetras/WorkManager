using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.BaseClasses;
using WorkManager.DAL.Entities.Interfaces;
using WorkManager.DAL.Enums;

namespace WorkManager.DAL.Entities
{
	public class TaskEntity : IEntity
	{
		public TaskEntity()
		{
		}
        [Key]
        public Guid Id { get; set; }
		public DateTime ActualDateTime { get; set; }
        [StringLength(30)]
		public string Name { get; set; }
        [StringLength(300)]
        public string Description { get; set; }
		public DateTime TaskDoneDateTime { get; set; }

		[Required]
		public Guid StateId { get; set; }

		[ForeignKey(nameof(StateId))]
		public virtual KanbanStateEntity State { get; set; }

		[Required]
		public Guid TaskGroupId { get; set; }

		[ForeignKey(nameof(TaskGroupId))]
		public virtual TaskGroupEntity TaskGroup { get; set; }
		public EPriority Priority { get; set; }
		public TimeSpan WorkTime { get; set; }
	}
}