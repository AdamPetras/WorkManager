using System;
using WorkManager.DAL.Entities;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class KanbanStateModel:ModelBase, IKanbanStateModel
	{
		public KanbanStateModel() : base(Guid.Empty)
		{

		}

		public KanbanStateModel(Guid id, string name, int stateOrder, string iconName, Guid taskGroupId) : base(id)
		{
			Name = name;
			StateOrder = stateOrder;
			IconName = iconName;
			TaskGroupId = taskGroupId;
		}

		public string Name { get; set; }
		public int StateOrder { get; set; }
		public string IconName { get; set; }
		public Guid TaskGroupId { get; set; }

		public bool Equals(IKanbanStateModel other)
		{
			return Equals((KanbanStateModel)other);
		}

		protected bool Equals(KanbanStateModel other)
		{
			return Name == other.Name && StateOrder == other.StateOrder && IconName == other.IconName && TaskGroupId == other.TaskGroupId;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((KanbanStateModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, StateOrder, IconName, TaskGroupId);
		}
	}
}