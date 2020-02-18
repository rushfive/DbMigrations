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
	public class MongoMigrationContext : MigrationContext
	{
		public AdaptiveMongoDbContext DbContext { get; private set; }
		public readonly MongoMigrationOptions Options;
		public readonly MongoMigrationInsights Insights;
		private readonly Stopwatch _stopwatch;

		public MongoMigrationContext(
			MongoMigrationOptions options,
			VersionedDatabase database,
			DbVersion version)
			: base(database, version)
		{
			Options = options ?? throw new ArgumentNullException(nameof(options));
			Insights = new MongoMigrationInsights();
			_stopwatch = new Stopwatch();
		}

		internal void SetDbContext(AdaptiveMongoDbContext dbContext)
		{
			DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		internal void StartStopwatch() => _stopwatch.Start();
		internal void StopStopwatch() => _stopwatch.Stop();

		internal void StartTransaction()
		{
			if (Options.UseTransaction)
				DbContext.StartTransaction();
		}
	}

	public class MongoMigrationInsights
	{
		public MigrationResultType? MigrationResult { get; private set; }

	}
}
