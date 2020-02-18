using R5.DbMigrations.Domain;
using R5.DbMigrations.Domain.Migrations;
using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Database;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo.Migrations
{
	// should have a new instance per migration
	public class MongoMigrationContext : MigrationContext
	{
		public readonly MongoMigrationOptions Options;
		public readonly AdaptiveMongoDbContext DbContext;
		//public readonly MongoMigrationInsights Insights;
		//private readonly Stopwatch _stopwatch;

		public MongoMigrationContext(
			MongoMigrationOptions options,
			VersionedDatabase database,
			DbVersion version,
			AdaptiveMongoDbContext dbContext)
			: base(database, version)
		{
			Options = options ?? throw new ArgumentNullException(nameof(options));
			DbContext = dbContext;
			//Insights = new MongoMigrationInsights();
			//_stopwatch = new Stopwatch();
		}

		internal MongoMigrationContext ForMigration(DbVersion version, AdaptiveMongoDbContext dbContext)
			=> new MongoMigrationContext(Options, Database, version, dbContext);

		//internal void StartTransaction()
		//{
		//	if (Options.UseTransaction)
		//		DbContext.StartTransaction();
		//}
	}

	//public class MongoMigrationInsights
	//{
	//	public MigrationResultType? MigrationResult { get; private set; }

	//}
}
