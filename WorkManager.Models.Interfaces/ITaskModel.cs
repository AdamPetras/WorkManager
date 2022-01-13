﻿using System;
using System.Collections.Generic;
using WorkManager.DAL.Enums;

namespace WorkManager.Models.Interfaces
{
	public interface ITaskModel : IModel, IEquatable<ITaskModel>
	{
		DateTime ActualDateTime { get; set; }
		string Name { get; set; }
		uint ImagesCount { get; set; }
		string Description { get; set; }
		DateTime TaskDoneDateTime { get; set; }
		Guid TaskGroupId { get; set; }
        Guid StateId { get; set; }
		EPriority Priority { get; set; }
		TimeSpan WorkTime { get; set; }
	}
}