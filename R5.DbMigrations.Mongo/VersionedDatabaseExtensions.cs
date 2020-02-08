using R5.DbMigrations.Domain;
using R5.DbMigrations.Domain.Migrations;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.DbMigrations.Mongo
{
	public static class VersionedDatabaseExtensions
	{
		public static void AddMigrationLog(this VersionedDatabase db,
			MongoMigration migration, MigrationLog.ApplyAttempt attempt)
		{
			var log = db.AppliedMigrations.SingleOrDefault(m => m.DbVersion == migration.Version);
			if (log != null)
			{
				log.History.Add(attempt);
			}
			else
			{
				MigrationLog newLog = migration.CreateLogFor(attempt);
				db.AppliedMigrations.Add(newLog);
			}
		}
	}
}
