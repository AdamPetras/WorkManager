namespace WorkManager.Models.Interfaces
{
	public interface IImageModel: IModel
	{
		public string Path { get; set; }
		public string Description { get; set; }
		public ITaskModel Task { get; set; }
	}
}