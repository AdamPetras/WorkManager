using System;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models.BaseClasses
{
	public abstract class ModelBase:IModel
	{
		protected ModelBase(Guid id)
		{
			Id = id;
		}
		public Guid Id { get; set; }
	}
}