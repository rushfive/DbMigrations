//using Microsoft.Extensions.Logging;
//using R5.DbMigrations.Engine.Processing;
//using R5.DbMigrations.Mongo.Migrations;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace R5.DbMigrations.Mongo.Processing.Stages
//{
//	public class RunMigrationStage_ : MongoMigrationStage
//	{
//		protected override string Description
//			=> $"Running migration '{_context.MigrationVersion}'";

//		private readonly MongoMigration _migration;

//		public RunMigrationStage_(
//			MongoMigration migration,
//			MongoMigrationContext context,
//			ILoggerFactory loggerFactory)
//			: base(context, loggerFactory)
//		{
//			_migration = migration;
//		}

//		protected override async Task<NextCommand> ProcessAsync(MongoMigrationContext context, object input)
//		{
//			try
//			{
//				await _migration.ApplyAsync(context);
//				return NextCommand.Continues;
//			}
//			catch (Exception ex)
//			{
//				if (context.Options.RetryWithoutTransactionOnFail)
//				{
//					_logger.LogWarning($"Migration failed. Aborting current transaction and "
//						+ "retrying without one.");
//					await context.DbContext.AbortTransactionAsync();
//					await _migration.ApplyAsync(context);
//					return NextCommand.Continues;
//				}
//				else
//				{
//					_logger.LogError(ex, "Failed to apply migrations. Might need to re-run without transactions.");
//					throw;
//				}
//			}
//		}
//	}
//}
