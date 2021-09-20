using System;

namespace WorkManager.Models.Interfaces
{
	public interface IKanbanStateModel:IModel, IEquatable<IKanbanStateModel>
	{
		string Name { get; set; }
		int StateOrder { get; set; }
		string IconName { get; set; }
	}
}