namespace WorkManager.Models.Interfaces
{
	public interface IKanbanTaskGroupModel : IModel
	{
		ITaskGroupModel TaskGroup { get; set; }
		IKanbanStateModel Kanban { get; set; }
	}
}