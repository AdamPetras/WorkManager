using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Entities
{
	public class KanbanStateEntity : IEntity
	{
        [Key]
        public Guid Id { get; set; }
        [StringLength(20)]
		public string Name { get; set; }
        public int StateOrder { get; set; }
        [StringLength(10)]
		public string IconName { get; set; }
		[Required]
		public Guid TaskGroupId { get; set; }

		[ForeignKey(nameof(TaskGroupId))]
		public virtual TaskGroupEntity TaskGroup { get; set; }
	}
}