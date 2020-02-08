using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations
{
	public static class IEnumerableExtensions
	{
		public static IEnumerable<T> ForEach<T>(
			this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach (T item in enumerable) action(item);
			return enumerable;
		}
	}
}
