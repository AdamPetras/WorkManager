using WorkManager.Models.Interfaces;

namespace WorkManager.BL.DialogEvents
{
	public class AddAfterDialogCloseDialogEvent<T>: IDialogEvent where T:IModel
	{
		public AddAfterDialogCloseDialogEvent(T value)
		{
			Value = value;
		}

		public T Value { get; }
	}
}