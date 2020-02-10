using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations
{
	public static class TypeExtensions
	{
		public static bool IsDirectDerivationOf<T>(this Type t)
			=> t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(T));

		public static bool IsDerivedFrom<T>(this Type t)
			=> t != typeof(T) && t.IsAssignableFrom(typeof(T));
	}
}
