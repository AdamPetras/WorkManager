using System;
using System.Collections.Generic;
using System.Linq;
using WorkManager.BL.DialogEvents;
using WorkManager.Models.Interfaces;
using Xamarin.Forms.Internals;

namespace WorkManager.BL.Services
{
	public class DialogEventService
	{
		public void OnRaiseDialogEvent<T>(IDialogEvent dialogEvent, IList<T> lst) where T:IModel
		{
			switch (dialogEvent)
			{
				case AddAfterDialogCloseDialogEvent<T> addAfterDialogCloseDialogEvent:
					lst.Add(addAfterDialogCloseDialogEvent.Value);
					break;
				case UpdateAfterDialogCloseDialogEvent<T> updateAfterDialogCloseDialogEvent:

					int index = lst.IndexOf(lst.Single(s => s.Id == updateAfterDialogCloseDialogEvent.Value.Id));
					lst.RemoveAt(index);
					lst.Insert(index, updateAfterDialogCloseDialogEvent.Value);
					break;
				case RemoveAfterDialogCloseDialogEvent<T> removeAfterDialogCloseDialogEvent:
					lst.Remove(lst.Single(s => s.Id == removeAfterDialogCloseDialogEvent.Value.Id));
					break;
				case null:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(dialogEvent));
			}
		}
	}
}