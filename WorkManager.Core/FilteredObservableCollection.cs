using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WorkManager.Core.Annotations;

namespace WorkManager.Core
{
	public class FilteredObservableCollection<T>: INotifyPropertyChanged, IDisposable
	{
		public FilteredObservableCollection(IEnumerable<T> enumerable)
		{
			WholeCollection = new System.Collections.ObjectModel.ObservableCollection<T>(enumerable ?? throw new ArgumentException());
			Filter = (s) => true;
		}

		public FilteredObservableCollection(System.Collections.ObjectModel.ObservableCollection<T> wholeCollection)
		{
			WholeCollection = wholeCollection ?? throw new ArgumentException();
			Filter = (s) => true;
		}

		public FilteredObservableCollection(System.Collections.ObjectModel.ObservableCollection<T> wholeCollection, Func<T, bool> filter)
		{
			WholeCollection = wholeCollection ?? throw new ArgumentException();
			Filter = filter ?? throw new ArgumentException();
		}

		public FilteredObservableCollection(IEnumerable<T> enumerable, Func<T, bool> filter)
		{
			WholeCollection = new System.Collections.ObjectModel.ObservableCollection<T>(enumerable ?? throw new ArgumentException());
			Filter = filter ?? throw new ArgumentException();
		}

		private Func<T, bool> _filter;
		public Func<T, bool> Filter 
		{ 
			get => _filter;
			set
			{
				_filter = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(FilteredCollection));
			}
		}

		private System.Collections.ObjectModel.ObservableCollection<T> _wholeCollection;
		public System.Collections.ObjectModel.ObservableCollection<T> WholeCollection
		{
			get => _wholeCollection;
			set
			{
				if (_wholeCollection == value) return;
				_wholeCollection = value;
				_wholeCollection.CollectionChanged += OnWholeCollectionChanged;
				OnPropertyChanged();
				OnPropertyChanged(nameof(FilteredCollection));
			}
		}

		public System.Collections.ObjectModel.ObservableCollection<T> FilteredCollection => new System.Collections.ObjectModel.ObservableCollection<T>(_wholeCollection.Where(Filter));

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void Dispose()
		{
			WholeCollection.CollectionChanged -= OnWholeCollectionChanged;
		}

		private void OnWholeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			OnPropertyChanged(nameof(FilteredCollection));
		}
	}
}