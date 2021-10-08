namespace WorkManager.Models.Interfaces
{
	public interface IDraggableModel : IModel
	{
		bool IsBeingDragged { get; set; }
		bool IsBeingDraggedOverFromTop { get; set; }
		bool IsBeingDraggedOverFromBottom { get; set; }
    }
}