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

		public bool Equals(IImageModel other)
		{
			return Equals((ImageModel)other);
		}

		protected bool Equals(ImageModel other)
		{
			return Path == other.Path && Description == other.Description && Equals(Task, other.Task);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == this.GetType() && Equals((ImageModel) obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Path, Description, Task);
		}
	}
}