namespace WorkManager.Models.Interfaces
{
	public interface IKanbanStateModel:IModel
	{
		string Name { get; set; }
		int StateOrder { get; set; }
		string IconName { get; set; }
	}
}