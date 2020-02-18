using Microsoft.Extensions.Logging;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo.Processing.Stages
{
	public class PreMigrationTasksStage : MongoMigrationStage
	{
		protected override string Description
			=> "Starting timer" + (_context.Options.UseTransaction ? " and starting transaction" : "");

		public PreMigrationTasksStage(
			MongoMigrationContext context,
			ILoggerFactory loggerFactory)
			: base(context, loggerFactory)
		{

		}

		protected override Task<NextCommand> ProcessAsync(MongoMigrationContext context, object input)
		{
			context.StartTransaction();
			context.StartStopwatch();
			return NextCommand.Continues.AsAwaitable();
		}
	}
}
