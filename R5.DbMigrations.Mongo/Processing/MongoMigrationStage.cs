using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo.Processing
{
	public class MongoMigrationStage : Stage<MongoPipelineContext, MongoMigrationContext>
	{
		public MongoMigrationStage(MongoPipelineContext context)
			: base(context)
		{

		}

		protected override Task<NextCommand> ProcessAsync(MongoMigrationContext context, object input)
		{
			throw new NotImplementedException();
		}
	}
}
