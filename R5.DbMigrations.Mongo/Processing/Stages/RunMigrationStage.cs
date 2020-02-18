using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using R5.DbMigrations.Domain.Migrations;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Database;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo.Processing.Stages
{
	public class RunMigrationStage : Stage<MongoMigrationContext>
	{
		private readonly ILogger _logger;
		private readonly MongoMigration _migration;

		public RunMigrationStage(
			ILogger logger,
			MongoMigration migration)
		{
			_logger = logger;
			_migration = migration;
		}

		protected override async Task<NextCommand> ProcessAsync(MongoMigrationContext context)
		{
			_logger.LogInformation($"Begin running mongo migration '{_migration.Version}'");

			bool useTransaction = context.Options.UseTransaction;
			bool retryWithoutTransaction = context.Options.RetryWithoutTransactionOnFail;

			MigrationLog.ApplyAttempt attempt = await RunMigrationAsync(context, useTransaction);

			// initial failed attempt used transaction, and processing is set to retry
			bool shouldRetry = attempt.Result == MigrationResultType.Error
				&& useTransaction
				&& retryWithoutTransaction;

			if (shouldRetry)
			{
				_logger.LogWarning("Initial migration attempt failed. Retrying without transaction.");
				attempt = await RunMigrationAsync(context, useTransaction: false);
			}

			_logger.LogInformation($"Saving attempt log to database record.");
			await SaveMigrationAttemptAsync(context, attempt, _migration);

			switch (attempt.Result)
			{
				case MigrationResultType.Error:
					_logger.LogError($"Migration '{_migration.Version}' failed so will not continue. Check db migration history for more details.");
					return NextCommand.Ends;
				case MigrationResultType.Completed:
					_logger.LogInformation($"Migration ''{_migration.Version}' completed after {attempt.ElapsedTimeSeconds} seconds.");
					return NextCommand.Continues;
				default:
					throw new InvalidOperationException($"'{attempt.Result}' is an unknown migration result type.");
			}
		}

		private async Task<MigrationLog.ApplyAttempt> RunMigrationAsync(MongoMigrationContext context, bool useTransaction)
		{
			AdaptiveMongoDbContext dbContext = GetMongoDbContext(context.Database.ConnectionString, useTransaction);
			var sw = new Stopwatch();
			var startTime = DateTime.UtcNow;
			MigrationResultType? result = null;
			object additionalContext = null;

			try
			{
				MongoMigrationContext migrationContext = context.ForMigration(
					   _migration.Version,
					   dbContext);

				dbContext.StartTransaction();
				sw.Start();

				await _migration.ApplyAsync(context);

				await dbContext.CommitTransactionAsync();
				result = MigrationResultType.Completed;
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Migration failed.");

				await dbContext.AbortTransactionAsync();
				
				result = MigrationResultType.Error;
				additionalContext = new
				{
					Exception = ex,
					Message = ex.Message,
					StackTrace = ex.ToString()
				};
			}
			finally
			{
				sw.Stop();
			}

			return new MigrationLog.ApplyAttempt
			{
				Start = startTime,
				ElapsedTimeSeconds = sw.Elapsed.TotalSeconds,
				Result = result.Value,
				AdditionalContext = additionalContext
			};
		}

		private AdaptiveMongoDbContext GetMongoDbContext(string connectionString, bool useTransaction)
			=> useTransaction
				? AdaptiveMongoDbContext.WithTransaction(GetMongoDatabase(connectionString))
				: AdaptiveMongoDbContext.WithoutTransaction(GetMongoDatabase(connectionString));

		private static IMongoDatabase GetMongoDatabase(string connectionString)
		{
			var url = MongoUrl.Create(connectionString);
			var clientSettings = MongoClientSettings.FromUrl(url);
			var client = new MongoClient(clientSettings);

			// todo: configure this too
			var dbSettings = new MongoDatabaseSettings();
			var database = client.GetDatabase(url.DatabaseName, dbSettings);
			return database;
		}

		private async Task SaveMigrationAttemptAsync(
			MongoMigrationContext context, 
			MigrationLog.ApplyAttempt attempt, 
			MongoMigration migration)
		{
			AdaptiveMongoDbContext dbContext = GetMongoDbContext(context.Database.ConnectionString, useTransaction: false);

			var filter = MongoFilters.MigrationLog.SingleMatchingVersion(migration.Version);
			var collection = dbContext.GetCollection<MigrationLog>(MigrationLog.CollectionName);

			MigrationLog log = await collection.Find(filter).SingleOrDefaultAsync();
			if (log != null)
			{
				log.History.Add(attempt);
				ReplaceOneResult result = await collection.ReplaceOneAsync(filter, log);
				if (result.ModifiedCount != 1)
					throw new InvalidOperationException($"Adding attempt document to migration '{migration.Version}' failed during replace op.");
			}
			else
			{
				log = new MigrationLog(
					migration.Version.Version,
					migration.Version.YearQuarter,
					migration.Description,
					new List<MigrationLog.ApplyAttempt> { attempt });
				await collection.InsertOneAsync(log);
			}
		}
	}
}
