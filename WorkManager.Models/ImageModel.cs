using System;
using WorkManager.Models.BaseClasses;
using WorkManager.Models.Interfaces;

namespace WorkManager.Models
{
	public class ImageModel:ModelBase, IImageModel
	{
		public ImageModel() : base(Guid.Empty)
		{
			
		}

		public ImageModel(Guid id, string path, string description, ITaskModel task) : base(id)
		{
			Path = path;
			Description = description;
			Task = task;
		}

		public string Path { get; set; }
		public string Description { get; set; }
		public ITaskModel Task { get; set; }
	}
}