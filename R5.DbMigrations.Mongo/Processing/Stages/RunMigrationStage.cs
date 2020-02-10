using Microsoft.Extensions.Logging;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo.Processing.Stages
{
	public class RunMigrationStage : MongoMigrationStage
	{
		protected override string Description
			=> $"Running migration '{_context.MigrationVersion}'";

		private readonly MongoMigration _migration;

		public RunMigrationStage(
			MongoMigration migration,
			MongoMigrationContext context,
			ILoggerFactory loggerFactory)
			: base(context, loggerFactory)
		{
			_migration = migration;
		}

		protected override async Task<NextCommand> ProcessAsync(MongoMigrationContext context, object input)
		{
			try
			{
				await _migration.ApplyAsync(context);
			}
			catch (Exception ex)
			{

			}

			throw new NotImplementedException();
		}
	}
}
