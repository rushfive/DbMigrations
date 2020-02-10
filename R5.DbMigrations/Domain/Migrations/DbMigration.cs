using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Domain.Migrations
{
	public abstract class DbMigration<TContext>
		: IComparable<DbMigration<TContext>>, IEquatable<DbMigration<TContext>>
		where TContext : MigrationContext
	{
		public abstract DbVersion Version { get; }
		public abstract string Description { get; }

		public abstract Task ApplyAsync(TContext migrationContext);

		public int CompareTo([AllowNull] DbMigration<TContext> other)
		{
			if (other == null) return 1;
			return Version.CompareTo(other.Version);
		}

		public bool Equals([AllowNull] DbMigration<TContext> other)
		{
			if (other == null) return false;
			return Version == other.Version;
		}

		public override bool Equals(object obj)
		{
			var other = obj as DbMigration<TContext>;
			if (other == null) return false;
			return Equals(other);
		}

		public override int GetHashCode() => Version.GetHashCode();

		public override string ToString() => Version.ToString();
	}
}
