using System;

namespace WorkManager.Models.Interfaces
{
	public interface IKanbanTaskGroupModel : IModel, IEquatable<IKanbanTaskGroupModel>
	{
		ITaskGroupModel TaskGroup { get; set; }
		IKanbanStateModel Kanban { get; set; }
	}
}