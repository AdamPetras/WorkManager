using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkManager.Models.Interfaces
{
	public interface ITaskGroupModel : IModel, IEquatable<ITaskGroupModel>
	{
        [StringLength(30)]
		string Name { get; set; }
        [StringLength(300)]
        string Description { get; set; }
        uint TasksCount { get; set; }
		Guid UserId { get; set; }
	}
}