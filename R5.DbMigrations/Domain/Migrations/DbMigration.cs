using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Domain.Migrations
{
	public abstract class DbMigration
		: IComparable<DbMigration>, IEquatable<DbMigration>
	{
		public abstract DbVersion Version { get; }
		public abstract string Description { get; }

		public abstract Task ApplyAsync(object migrationContext);

		public int CompareTo([AllowNull] DbMigration other)
		{
			if (other == null) return 1;
			return Version.CompareTo(other.Version);
		}

		public bool Equals([AllowNull] DbMigration other)
		{
			if (other == null) return false;
			return Version == other.Version;
		}

		public override bool Equals(object obj)
		{
			var other = obj as DbMigration;
			if (other == null) return false;
			return Equals(other);
		}

		public override int GetHashCode() => Version.GetHashCode();

		public override string ToString() => Version.ToString();
	}
}
