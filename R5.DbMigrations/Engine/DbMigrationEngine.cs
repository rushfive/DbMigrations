﻿using R5.DbMigrations.Domain;
using R5.DbMigrations.Domain.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Engine
{
	public abstract class DbMigrationEngine<TMigration>
		where TMigration : DbMigration
	{
		protected readonly VersionedDatabase _database;

		protected DbMigrationEngine(VersionedDatabase database)
		{
			_database = database ?? throw new ArgumentNullException(nameof(database));
		}

		public abstract Task RunAsync(IEnumerable<TMigration> allExistingMigrations);
	}
}
