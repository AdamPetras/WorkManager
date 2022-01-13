using System;

namespace WorkManager.Models.Interfaces
{
	public interface IImageModel: IModel, IEquatable<IImageModel>
	{
		public string Path { get; set; }
		public string Description { get; set; }
		public Guid TaskId { get; set; }
	}
}