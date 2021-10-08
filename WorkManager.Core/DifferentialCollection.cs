using System.Collections.Generic;

namespace WorkManager.Core
{
	public class DifferentialCollection<T>
	{
		public IEnumerable<T> AddEnumerable { get; }
		public IEnumerable<T> DeleteEnumerable { get; }
		public IEnumerable<T> UpdateEnumerable { get; }

		public DifferentialCollection(IEnumerable<T> addEnumerable, IEnumerable<T> deleteEnumerable)
		{
			AddEnumerable = addEnumerable;
			DeleteEnumerable = deleteEnumerable;
		}

		public DifferentialCollection(IEnumerable<T> addEnumerable, IEnumerable<T> deleteEnumerable, IEnumerable<T> updateEnumerable)
		{
			AddEnumerable = addEnumerable;
			DeleteEnumerable = deleteEnumerable;
			UpdateEnumerable = updateEnumerable;
		}
	}
}