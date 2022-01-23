using System;
using System.ComponentModel.DataAnnotations;

namespace WorkManager.Models.Interfaces
{
	public interface IImageModel: IModel, IEquatable<IImageModel>
	{
        [StringLength(200)]
		public string Path { get; set; }
        [StringLength(300)]
		public string Description { get; set; }
        public Guid TaskId { get; set; }
	}
}