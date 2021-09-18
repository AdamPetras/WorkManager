using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WorkManager.DAL.Enums;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class TaskModel: ModelBase, ITaskModel
	{
		public TaskModel() : base(Guid.Empty)
		{
		}

		public TaskModel([NotNull] ITaskModel task): base(task.Id)
		{
			ActualDateTime = task.ActualDateTime;
			Name = task.Name;
			Description = task.Description;
			TaskDoneDateTime = task.TaskDoneDateTime;
			TaskGroup = task.TaskGroup;
			State = task.State;
			Priority = task.Priority;
			WorkTime = task.WorkTime;
		}

		public TaskModel(Guid id, DateTime actualDateTime, string name, string description, DateTime taskDoneDateTime,
			ITaskGroupModel taskGroup, IKanbanStateModel state, EPriority priority, TimeSpan workTime) : base(id)
		{
			ActualDateTime = actualDateTime;
			Name = name;
			Description = description;
			TaskDoneDateTime = taskDoneDateTime;
			TaskGroup = taskGroup;
			State = state;
			Priority = priority;
			WorkTime = workTime;
		}

		public DateTime ActualDateTime { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime TaskDoneDateTime { get; set; }
		public ITaskGroupModel TaskGroup { get; set; }
		public IKanbanStateModel State { get; set; }
		public EPriority Priority { get; set; }
		public TimeSpan WorkTime { get; set; }
	}
}
