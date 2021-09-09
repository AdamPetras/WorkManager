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
	}
}