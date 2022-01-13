using System;
using System.Collections;
using System.Collections.Generic;

namespace WorkManager.Models.Interfaces
{
	public interface ITaskGroupModel : IModel, IEquatable<ITaskGroupModel>
	{
		string Name { get; set; }
		string Description { get; set; }
		uint TasksCount { get; set; }
		Guid UserId { get; set; }
	}
}