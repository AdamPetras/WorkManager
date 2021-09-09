using System;
using System.Collections.Generic;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class TaskGroupModel : ModelBase, ITaskGroupModel
	{
		public TaskGroupModel() : base(Guid.Empty)
		{

		}

		public TaskGroupModel(Guid id, string name, string description, IUserModel user) : base(id)
		{
			Name = name;
			Description = description;
			User = user;
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public IUserModel User { get; set; }
	}
}