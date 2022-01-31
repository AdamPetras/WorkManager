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
		public void OnRaiseDialogEvent<T>(IDialogEvent dialogEvent, IList<T> lst, Func<T, bool> condition = null) where T:IModel
        {
            switch (dialogEvent)
			{
				case AddAfterDialogCloseDialogEvent<T> addAfterDialogCloseDialogEvent:
					if(condition != null && condition(addAfterDialogCloseDialogEvent.Value))
                    {
                        lst.Add(addAfterDialogCloseDialogEvent.Value);
                    }
                    break;
				case UpdateAfterDialogCloseDialogEvent<T> updateAfterDialogCloseDialogEvent:
                    if (condition != null && condition(updateAfterDialogCloseDialogEvent.Value))
                    {
                        int index = lst.IndexOf(lst.Single(s => s.Id == updateAfterDialogCloseDialogEvent.Value.Id));
                        lst.RemoveAt(index);
                        lst.Insert(index, updateAfterDialogCloseDialogEvent.Value);
                    }
                    break;
				case RemoveAfterDialogCloseDialogEvent<T> removeAfterDialogCloseDialogEvent:
                    if (condition != null && condition(removeAfterDialogCloseDialogEvent.Value))
                    {
                        lst.Remove(lst.Single(s => s.Id == removeAfterDialogCloseDialogEvent.Value.Id));
                    }
                    break;
				case null:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(dialogEvent));
			}
        }
	}
}