using System;

namespace WorkManager.DAL.Entities.Interfaces
{
	public interface IEntity
	{
		Guid Id { get; set; }
	}
}