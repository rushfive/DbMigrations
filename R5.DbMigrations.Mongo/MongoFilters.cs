using MongoDB.Driver;
using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Text;
using DomainMigrationLog = R5.DbMigrations.Domain.Migrations.MigrationLog;

namespace R5.DbMigrations.Mongo
{
	public static class MongoFilters
	{
		public static class MigrationLog
		{
			public static FilterDefinition<DomainMigrationLog>
				SingleMatchingVersion(DbVersion dbVersion)
					=> _builder.Eq(l => l.SemVer, dbVersion.Version.ToString());

			private readonly static FilterDefinitionBuilder<DomainMigrationLog> _builder
				= Builders<DomainMigrationLog>.Filter;
		}
	}
}
