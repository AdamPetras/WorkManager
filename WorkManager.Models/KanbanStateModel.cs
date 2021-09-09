using System;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class KanbanStateModel:ModelBase, IKanbanStateModel
	{
		public KanbanStateModel() : base(Guid.Empty)
		{

		}

		public KanbanStateModel(Guid id, string name, int stateOrder, string iconName) : base(id)
		{
			Name = name;
			StateOrder = stateOrder;
			IconName = iconName;
		}

		public string Name { get; set; }
		public int StateOrder { get; set; }
		public string IconName { get; set; }
	}
}