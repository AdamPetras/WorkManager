using System;
using System.ComponentModel.DataAnnotations;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Entities.BaseClasses
{
	public abstract class EntityBase : IEntity
	{
		protected EntityBase()
		{
		}
        [Key]
        public Guid Id { get; set; }
	}
}