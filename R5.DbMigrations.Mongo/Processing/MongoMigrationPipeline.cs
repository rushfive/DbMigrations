using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using System.Collections.Generic;
using System.Linq;

namespace R5.DbMigrations.Mongo.Processing
{
	public class MongoMigrationPipeline : Pipeline<MongoMigrationContext>
	{
		public MongoMigrationPipeline(
			List<MongoMigrationStage> stages,
			MongoMigrationContext context)
			: base(stages.Cast<Stage<MongoMigrationContext>>().ToList(), context)
		{
		}
	}
}
