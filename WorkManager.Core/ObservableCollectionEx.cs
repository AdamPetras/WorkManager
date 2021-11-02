using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace WorkManager.Core
{
	public partial class ObservableCollectionEx<T> : System.Collections.ObjectModel.ObservableCollection<T>
	{
		public ObservableCollectionEx() : base()
		{
		}

		public ObservableCollectionEx(List<T> list) : base(((list != null) ? new List<T>(list.Count) : list) ??
		                                                   throw new ArgumentNullException("list"))
		{
			CopyFrom(list);
		}

		public ObservableCollectionEx(IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException("collection");
			CopyFrom(collection);
		}


		private void CopyFrom(IEnumerable<T> collection)
		{
			IList<T> items = Items;
			if (collection != null)
			{
				using (IEnumerator<T> enumerator = collection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						items.Add(enumerator.Current);
					}
				}
			}
		}

		public void AddRange(IEnumerable<T> collection)
		{
			foreach (var i in collection)
			{
				Items.Add(i);
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Add, i));
			}
		}
	}
}