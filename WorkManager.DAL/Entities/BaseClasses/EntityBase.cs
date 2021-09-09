using System;
using WorkManager.DAL.Entities.Interfaces;

namespace WorkManager.DAL.Entities.BaseClasses
{
	public abstract class EntityBase : IEntity
	{
		protected EntityBase()
		{
		}

		public Guid Id { get; set; }
	}
}