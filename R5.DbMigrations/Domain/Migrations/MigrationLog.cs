using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace R5.DbMigrations.Domain.Migrations
{
	public class MigrationLog
		: IComparable<MigrationLog>, IEquatable<MigrationLog>
	{
		public SemanticVersion SemVer { get; set; }
		public YearQuarterVersion Version { get; set; }
		public string Description { get; set; }
		public List<ApplyAttempt> History { get; set; }

		public const string CollectionName = "MigrationLogs";

		public MigrationLog(
			SemanticVersion semVer,
			YearQuarterVersion version,
			string description,
			List<ApplyAttempt> history)
		{
			SemVer = semVer
				?? throw new ArgumentNullException(nameof(semVer));
			Version = version
				?? throw new ArgumentNullException(nameof(version));
			Description = description;
			History = history
				?? throw new ArgumentNullException(nameof(history));
		}

		// enforce this record not created unless attempt persisted?
		public MigrationResultType LatestAttemptResult =>
			History.OrderByDescending(a => a.Start)
				.First().Result;

		public DbVersion DbVersion => new DbVersion(Version, SemVer);

		public class ApplyAttempt
		{
			public DateTime Start { get; set; }
			public double ElapsedTimeSeconds { get; set; }
			public MigrationResultType Result { get; set; }
			public object AdditionalContext { get; set; }
		}

		public int CompareTo([AllowNull] MigrationLog other)
		{
			if (other == null) return 1;
			return Version.CompareTo(other.Version);
		}

		public bool Equals([AllowNull] MigrationLog other)
		{
			if (other == null) return false;
			return Version.Equals(other.Version);
		}

		public override bool Equals(object obj)
		{
			var other = obj as MigrationLog;
			return Equals(other);
		}

		public override int GetHashCode() => Version.GetHashCode();

		public override string ToString() => $"{Version} ({LatestAttemptResult}) {Description}";
	}
}
