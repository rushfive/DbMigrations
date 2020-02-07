using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace R5.DbMigrations.Domain.Versioning
{
	public class DbVersion
		: IComparable<DbVersion>, IEquatable<DbVersion>
	{
		public readonly YearQuarterVersion YearQuarter;
		public readonly SemanticVersion Version;

		public DbVersion(
			YearQuarterVersion yearQuarter,
			SemanticVersion version)
		{
			YearQuarter = yearQuarter ?? throw new ArgumentNullException(nameof(yearQuarter), "YearQuarter version must be specified.");
			Version = version ?? throw new ArgumentNullException(nameof(version), "Semantic version must be specified.");
		}

		public static readonly DbVersion NewDatabase =
			new DbVersion(
				YearQuarterVersion.NewDatabase,
				SemanticVersion.NewDatabase);

		public int CompareTo([AllowNull] DbVersion other)
		{
			if (other == null) return 1;
			return Version.CompareTo(other.Version);
		}

		public static bool operator ==(DbVersion a, DbVersion b)
		{
			if (ReferenceEquals(a, b)) return true;
			if (ReferenceEquals(a, null)) return false;
			if (ReferenceEquals(b, null)) return false;
			return a.Equals(b);
		}

		public static bool operator !=(DbVersion a, DbVersion b)
			=> !(a == b);

		public static bool operator >(DbVersion a, DbVersion b)
		{
			if (ReferenceEquals(a, b)) return false;
			if (ReferenceEquals(a, null)) return false;
			if (ReferenceEquals(b, null)) return true;
			return a.CompareTo(b) == 1;
		}

		public static bool operator <(DbVersion a, DbVersion b)
		{
			if (ReferenceEquals(a, b)) return false;
			if (ReferenceEquals(a, null)) return true;
			if (ReferenceEquals(b, null)) return false;
			return a.CompareTo(b) == -1;
		}

		public bool Equals([AllowNull] DbVersion other)
		{
			if (other == null) return false;
			return Version.Equals(other.Version);
		}

		public override bool Equals(object obj)
		{
			var other = obj as DbVersion;
			if (other == null) return false;
			return Version.Equals(other.Version);
		}

		public override int GetHashCode() => Version.GetHashCode();

		public override string ToString() =>
			this == NewDatabase ?
				"New Database" : $"{Version} ({YearQuarter})";
	}
}
