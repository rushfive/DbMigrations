using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Mongo.Processing
{
	public class MongoMigrationPipeline : Pipeline<MongoPipelineContext, MongoMigrationContext>
	{
		public MongoMigrationPipeline(MongoMigrationStage headStage, MongoPipelineContext context)
			: base(headStage, context)
		{

		}
	}
}
