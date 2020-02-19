using R5.DbMigrations.Domain.Migrations;
using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanguageExt;
using static LanguageExt.Prelude;
using R5.DbMigrations.Engine.Processing;

namespace R5.DbMigrations.Domain
{
	public class VersionedDatabase
	{
		public readonly string Label;
		public readonly string ConnectionString;
		public readonly List<MigrationLog> AppliedMigrations;

		public DbVersion CurrentVersion { get; private set; }

		public VersionedDatabase(
			string label,
			string connectionString,
			IEnumerable<MigrationLog> appliedMigrations)
		{
			Label = label
				?? throw new ArgumentNullException(nameof(label), "Database label must be provided.");
			ConnectionString = connectionString
				?? throw new ArgumentNullException(nameof(connectionString), "Database connection string must be provided.");

			if (appliedMigrations == null)
				throw new ArgumentNullException(nameof(appliedMigrations), "Applied migrations must be provided.");

			var migrations = appliedMigrations.OrderByDescending(m => m.Version).ToList();
			AppliedMigrations = migrations;
			CurrentVersion = migrations.FirstOrDefault()?.DbVersion ?? DbVersion.NewDatabase;
		}

		public bool HasBeenInitialized => CurrentVersion != DbVersion.NewDatabase;

		public List<TMigration> GetRequiredMigrations<TMigration, TContext>(IEnumerable<TMigration> existingUpgrades)
			where TMigration : DbMigration<TContext>
			where TContext : MigrationContext
		{
			var completedVersions = AppliedMigrations
				.Where(m => m.LatestAttemptResult != MigrationResultType.Error)
				.Select(m => m.DbVersion)
				.ToHashSet();

			return existingUpgrades
				.Where(u => !completedVersions.Contains(u.Version))
				.OrderBy(u => u.Version)
				.Cast<TMigration>()
				.ToList();
		}

		public DbVersion GetLatestCompletedMigrationVersion()
			=> AppliedMigrations
				.Where(m => m.LatestAttemptResult == MigrationResultType.Completed)
				.Select(m => m.DbVersion)
				.OrderByDescending(v => v)
				.FirstOrDefault();

		//public MigrationLog.ApplyAttempt LatestAttempt =>
		//	AppliedMigrations.SelectMany(m => m.History)
		//		.OrderByDescending(a => a.Start)
		//		.FirstOrDefault();

		// replaced with db.AddMigrationLog()
		//public void UpdateWithAppliedMigration(
		//	MongoMigration migration, MigrationLog.ApplyAttempt attempt)
		//{
		//	var log = AppliedMigrations.SingleOrDefault(m => m.DbVersion == migration.Version);
		//	if (log != null)
		//	{
		//		log.History.Add(attempt);
		//	}
		//	else
		//	{
		//		var newLog = MigrationLog.CreateFrom(migration, attempt);
		//		AppliedMigrations.Add(newLog);
		//	}
		//}

		//public Option<MigrationLog.ApplyAttempt> LatestAttemptFor(DbVersion version)
		//{
		//	MigrationLog.ApplyAttempt attempt = AppliedMigrations.SingleOrDefault(m => m.DbVersion == version)?
		//		.History
		//		.OrderByDescending(a => a.Start)
		//		.FirstOrDefault();
		//	return attempt;
		//}
	}

	
}
