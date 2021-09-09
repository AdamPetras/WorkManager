using WorkManager.Models.Interfaces;

namespace WorkManager.BL.DialogEvents
{
	public class UpdateAfterDialogCloseDialogEvent<T> : IDialogEvent where T : IModel
	{
		public UpdateAfterDialogCloseDialogEvent(T value)
		{
			Value = value;
		}

		public T Value { get; }
	}
}