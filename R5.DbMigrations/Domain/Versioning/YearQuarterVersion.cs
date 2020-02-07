using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace R5.DbMigrations.Domain.Versioning
{
	public class YearQuarterVersion
		: IComparable<YearQuarterVersion>, IEquatable<YearQuarterVersion>
	{
		public readonly int Year;
		public readonly int Quarter;
		public readonly int Version;

		public YearQuarterVersion(int year, int quarter, int version)
		{
			//if (year < 0) throw new ArgumentOutOfRangeException(nameof(year), "Year cannot be less than 0.");
			//if (year < 2020 || year > 9999) throw new ArgumentOutOfRangeException(nameof(year), "Year must be a a valid 4 digit int greater than 2019.");
			//if (quarter < 1 || quarter > 4) throw new ArgumentOutOfRangeException(nameof(quarter), "Quarter can only have these values: 1,2,3,4");
			//if (version < 0) throw new ArgumentOutOfRangeException(nameof(version), "Version cannot be less than 0.");

			Year = year;
			Quarter = quarter;
			Version = version;
		}

		private YearQuarterVersion()
		{
			Year = -1;
			Quarter = -1;
			Version = -1;
		}

		public static readonly YearQuarterVersion NewDatabase = new YearQuarterVersion();

		public static readonly YearQuarterVersion MinValue = new YearQuarterVersion(2020, 1, 0);

		public static implicit operator YearQuarterVersion(string version) => Parse(version);

		public static YearQuarterVersion Parse(string version)
		{
			var split = version.Split('.');
			if (split.Length != 3)
				throw new InvalidOperationException($"'{version}' is not in the correct year-quarter version format.");
			if (!int.TryParse(split[0], out int year))
				throw new InvalidOperationException($"'{split[0]}' is not an int.");
			if (!int.TryParse(split[1], out int month))
				throw new InvalidOperationException($"'{split[1]}' is not an int.");
			if (!int.TryParse(split[2], out int v))
				throw new InvalidOperationException($"'{split[2]}' is not an int.");
			return new YearQuarterVersion(year, month, v);
		}

		public int CompareTo([AllowNull] YearQuarterVersion other)
		{
			if (other == null) return 1;

			if (Year > other.Year) return 1;
			if (Year < other.Year) return -1;

			if (Quarter > other.Quarter) return 1;
			if (Quarter < other.Quarter) return -1;

			if (Version > other.Version) return 1;
			if (Version < other.Version) return -1;

			return 0;
		}

		public bool Equals([AllowNull] YearQuarterVersion other)
		{
			if (other == null) return false;
			return CompareTo(other) == 0;
		}

		public override bool Equals(object obj)
		{
			var other = obj as YearQuarterVersion;
			if (other == null) return false;
			return Equals(other);
		}

		public override int GetHashCode() => (Year, Quarter, Version).GetHashCode();

		public override string ToString() => $"{Year}.{Quarter}.{Version}";
	}
}
