using System;
using System.Collections.Generic;
using WorkManager.DAL.Enums;

namespace WorkManager.Models.Interfaces
{
	public interface ITaskModel:IModel
	{
		DateTime ActualDateTime { get; set; }
		string Name { get; set; }
		public string Description { get; set; }
		DateTime TaskDoneDateTime { get; set; }
		ITaskGroupModel TaskGroup { get; set; }
		IKanbanStateModel State { get; set; }
		EPriority Priority { get; set; }
		TimeSpan WorkTime { get; set; }
	}
}