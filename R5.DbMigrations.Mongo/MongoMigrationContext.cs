using R5.DbMigrations.Domain.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Mongo
{
	public class MongoMigrationContext : MigrationContext<MongoMigrationOptions>
	{
		public AdaptiveMongoDatabase Database { get; set; }
	}
}
