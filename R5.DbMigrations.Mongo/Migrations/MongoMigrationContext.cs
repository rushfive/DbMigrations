using R5.DbMigrations.Domain;
using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Database;
using System;

namespace R5.DbMigrations.Mongo.Migrations
{
	// should have a new instance per migration
	public class MongoMigrationContext : MigrationContext
	{
		public readonly MongoMigrationOptions Options;
		public readonly AdaptiveMongoDbContext DbContext;

		private MongoMigrationContext(
			MongoMigrationOptions options,
			VersionedDatabase database,
			DbVersion version,
			AdaptiveMongoDbContext dbContext)
			: base(database, version)
		{
			Options = options ?? throw new ArgumentNullException(nameof(options));
			DbContext = dbContext;
		}

		public static MongoMigrationContext Initialize(
			MongoMigrationOptions options,
			VersionedDatabase database)
			=> new MongoMigrationContext(options, database, null, null);

		internal MongoMigrationContext ForMigration(DbVersion version, AdaptiveMongoDbContext dbContext)
			=> new MongoMigrationContext(Options, Database, version, dbContext);
	}
}
