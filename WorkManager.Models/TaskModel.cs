﻿using System;
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

		public TaskModel([NotNull] ITaskModel task): this(task.Id,task.ActualDateTime,task.Name,task.Description,task.TaskDoneDateTime,task.TaskGroup,task.State,task.Priority,task.WorkTime)
		{
			
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

		public bool Equals(ITaskModel other)
		{
			return Equals((TaskModel)other);
		}

		protected bool Equals(TaskModel other)
		{
			return ActualDateTime.Equals(other.ActualDateTime) && Name == other.Name && Description == other.Description &&
			       TaskDoneDateTime.Equals(other.TaskDoneDateTime) && Equals(TaskGroup, other.TaskGroup) && 
			       Equals(State, other.State) && Priority == other.Priority && WorkTime.Equals(other.WorkTime);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((TaskModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ActualDateTime, Name, Description, TaskDoneDateTime, TaskGroup, State, (int) Priority, WorkTime);
		}
	}
}
