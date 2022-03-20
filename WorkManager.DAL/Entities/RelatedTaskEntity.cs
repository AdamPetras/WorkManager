using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Entities
{
    public class RelatedTaskEntity : IEntity
    {
        public Guid Id { get; set; }
        [StringLength(30)]
        public string Name { get; set; }
        public virtual ICollection<TaskEntity> RelatedBy { get; set; }
    }
}