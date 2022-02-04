using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkManager.DAL.Enums;
using WorkManager.Xamarin.Core;

namespace WorkManager.Models.Interfaces
{
	public interface ITaskModel : IModel, IEquatable<ITaskModel>
	{
		DateTime ActualDateTime { get; set; }
        [StringLength(30)]
		string Name { get; set; }
        int ImagesCount { get; set; }
        [StringLength(300)]
		string Description { get; set; }
        DateTime TaskDoneDateTime { get; set; }
		Guid TaskGroupId { get; set; }
        Guid StateId { get; set; }
		LocalizedEnum Priority { get; set; }
		TimeSpan WorkTime { get; set; }
	}
}