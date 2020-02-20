using R5.DbMigrations.Domain.Migrations;
using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
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
	}
}
