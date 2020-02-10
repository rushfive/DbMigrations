using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Mongo.Processing
{
	internal class MongoMigrationPipelineResolver
	{
		private readonly MongoMigrationOptions _options;

		internal MongoMigrationPipelineResolver(MongoMigrationOptions options)
		{
			_options = options;
		}

		internal MongoMigrationPipeline CreateFor(MongoMigration migration)
		{

		}
	}
}
