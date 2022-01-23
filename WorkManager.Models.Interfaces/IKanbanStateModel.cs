using System;
using System.ComponentModel.DataAnnotations;

namespace WorkManager.Models.Interfaces
{
	public interface IKanbanStateModel:IModel, IEquatable<IKanbanStateModel>
	{
        [StringLength(20)]
		string Name { get; set; }
        int StateOrder { get; set; }
        [StringLength(10)]
		string IconName { get; set; }
        Guid TaskGroupId { get; set; }
	}
}