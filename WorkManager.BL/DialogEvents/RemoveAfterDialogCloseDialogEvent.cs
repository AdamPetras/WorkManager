using WorkManager.Models.Interfaces;

namespace WorkManager.BL.DialogEvents
{
	public class RemoveAfterDialogCloseDialogEvent<T> : IDialogEvent where T : IModel
	{
		public RemoveAfterDialogCloseDialogEvent(T value)
		{
			Value = value;
		}

		public T Value { get; }
	}
}