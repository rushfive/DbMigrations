using MongoDB.Driver;
using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Mongo.Processing
{
	public class MongoPipelineContext : PipelineContext<MongoMigrationContext>
	{
		public IClientSessionHandle TransactionSession { get; set; }

		public MongoPipelineContext(
			DbVersion version,
			IStageContextResolver<MongoMigrationContext> migrationContextResolver)
			: base(version, migrationContextResolver)
		{

		}

		// TODO: methods to interface w/ transactino session
	}
}
