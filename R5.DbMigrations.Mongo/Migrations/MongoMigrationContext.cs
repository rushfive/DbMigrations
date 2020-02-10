using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Database;

namespace R5.DbMigrations.Mongo.Migrations
{
	public class MongoMigrationContext : PipelineContext
	{
		public readonly AdaptiveMongoDatabase Database;

		public MongoMigrationContext(
			AdaptiveMongoDatabase database,
			DbVersion version)
			: base(version)
		{
			Database = database;
		}
	}
}
