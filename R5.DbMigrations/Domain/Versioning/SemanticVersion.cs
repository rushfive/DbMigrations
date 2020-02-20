using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace R5.DbMigrations.Domain.Versioning
{
	public class SemanticVersion
		: IComparable<SemanticVersion>, IEquatable<SemanticVersion>
	{
		public readonly int Major;
		public readonly int Minor;
		public readonly int Patch;

		public SemanticVersion(int major, int minor, int patch)
		{
			Major = major;
			Minor = minor;
			Patch = patch;
		}

		private SemanticVersion()
		{
			Major = -1;
			Minor = -1;
			Patch = -1;
		}

		public static readonly SemanticVersion NewDatabase = new SemanticVersion();

		public static readonly SemanticVersion MinValue = new SemanticVersion(0, 0, 0);

		public static implicit operator SemanticVersion(string version) => Parse(version);

		public static SemanticVersion Parse(string version)
		{
			var split = version.Split('.');
			if (split.Length != 3)
				throw new InvalidOperationException($"'{version}' is not in the correct SemVer format.");
			if (!int.TryParse(split[0], out int major))
				throw new InvalidOperationException($"'{split[0]}' is not an int.");
			if (!int.TryParse(split[1], out int minor))
				throw new InvalidOperationException($"'{split[1]}' is not an int.");
			if (!int.TryParse(split[2], out int patch))
				throw new InvalidOperationException($"'{split[2]}' is not an int.");
			return new SemanticVersion(major, minor, patch);
		}

		public int CompareTo([AllowNull] SemanticVersion other)
		{
			if (other == null) return 1;

			if (Major > other.Major) return 1;
			if (Major < other.Major) return -1;

			if (Minor > other.Minor) return 1;
			if (Minor < other.Minor) return -1;

			if (Patch > other.Patch) return 1;
			if (Patch < other.Patch) return -1;

			return 0;
		}

		public bool Equals([AllowNull] SemanticVersion other)
		{
			if (other == null) return false;
			return CompareTo(other) == 0;
		}

		public override bool Equals(object obj)
		{
			var other = obj as SemanticVersion;
			if (other == null) return false;
			return Equals(other);
		}

		public override int GetHashCode()
		{
			return (Major, Minor, Patch).GetHashCode();
		}

		public override string ToString() => $"{Major}.{Minor}.{Patch}";
	}
}
