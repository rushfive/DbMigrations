using Microsoft.Extensions.Logging;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo.Processing
{
	public abstract class MongoMigrationStage : Stage<MongoMigrationContext>
	{
		protected readonly ILogger _logger;
		protected abstract string Description { get; }

		public MongoMigrationStage(
			MongoMigrationContext context,
			ILoggerFactory loggerFactory)
			: base(context)
		{
			_logger = loggerFactory?.CreateLogger(nameof(MongoMigrationStage));//
		}

		protected override Action<MongoMigrationContext> OnStart => context =>
		{
			_logger.LogInformation($"[{this.GetType().Name}] {Description}");
		};
	}
}
