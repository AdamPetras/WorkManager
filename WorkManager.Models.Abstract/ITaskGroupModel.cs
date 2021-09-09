using System.Collections;
using System.Collections.Generic;

namespace WorkManager.Models.Interfaces
{
	public interface ITaskGroupModel : IModel
	{
		string Name { get; set; }
		string Description { get; set; }
		IUserModel User { get; set; }
	}
}