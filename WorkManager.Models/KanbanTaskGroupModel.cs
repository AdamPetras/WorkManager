using System;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class KanbanTaskGroupModel:ModelBase, IKanbanTaskGroupModel
	{
		public KanbanTaskGroupModel() : base(Guid.Empty)
		{

		}

		public KanbanTaskGroupModel(Guid id, ITaskGroupModel taskGroup, IKanbanStateModel kanban) : base(id)
		{
			TaskGroup = taskGroup;
			Kanban = kanban;
		}

		public ITaskGroupModel TaskGroup { get; set; }
		public IKanbanStateModel Kanban { get; set; }

		public bool Equals(IKanbanTaskGroupModel other)
		{
			return Equals((KanbanTaskGroupModel)other);
		}

		protected bool Equals(KanbanTaskGroupModel other)
		{
			return Equals(TaskGroup, other.TaskGroup) && Equals(Kanban, other.Kanban);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((KanbanTaskGroupModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(TaskGroup, Kanban);
		}
	}
}