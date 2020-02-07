using R5.DbMigrations.Domain.Migrations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace R5.DbMigrations.Mongo
{
	public abstract class MongoMigration : DbMigration
	{
		protected MongoMigrationContext MapContext(object migrationContext)
		{
			var context = migrationContext as MongoMigrationContext;

			Debug.Assert(context != null, 
				$"Context is not the expected '{nameof(MongoMigrationContext)}' type.");

			return context;
		}
	}

	public static class MongoMigrationExtensions
	{
		public static MigrationLog CreateLogFor(this MongoMigration migration, MigrationLog.ApplyAttempt attempt)
			=> new MigrationLog(
				migration.Version.Version,
				migration.Version.YearQuarter,
				migration.Description,
				new List<MigrationLog.ApplyAttempt> { attempt });
	}
}
