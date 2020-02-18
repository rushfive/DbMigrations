using R5.DbMigrations.Domain;
using R5.DbMigrations.Engine;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo.Processing
{
	public class MongoMigrationEngine : DbMigrationEngine<MongoMigration>
	{
		private readonly MongoMigrationPipelineResolver _pipelineResolver;

		public MongoMigrationEngine(
			VersionedDatabase database,
			MongoMigrationOptions options)
			: base(database)
		{
			if (options == null) throw new ArgumentNullException(nameof(options), "Mongo migration options must be provided.");
			_pipelineResolver = new MongoMigrationPipelineResolver(options, database);
		}


		public override async Task RunAsync(IEnumerable<MongoMigration> allExistingMigrations)
		{
			List<MongoMigration> requiredMigrations = _database.GetRequiredMigrations<MongoMigration>(allExistingMigrations).ToList();

			foreach(var m in requiredMigrations)
			{
				await RunMigrationAsync(m);
			}
		}

		private async Task RunMigrationAsync(MongoMigration migration)
		{
			MongoMigrationPipeline pipeline = _pipelineResolver.CreateFor(migration, _database);
		}
	}
}
