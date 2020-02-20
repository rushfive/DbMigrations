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
	public class RunMigrationStage : MongoMigrationStage
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

			AdaptiveMongoDbContext dbContext = GetMongoDbContext(
				context.Database.ConnectionString,
				useTransaction);

			MigrationLog.ApplyAttempt attempt = await RunMigrationAsync(
				context, 
				dbContext,
				useTransaction && _migration.CanUseTransaction);

			// initial failed attempt used transaction, and processing is set to retry
			bool shouldRetry = attempt.Result == MigrationResultType.Error
				&& useTransaction
				&& retryWithoutTransaction
				&& _migration.CanUseTransaction;

			if (shouldRetry)
			{
				_logger.LogWarning("Initial migration attempt failed. Retrying without transaction.");
				dbContext = GetMongoDbContext(
					context.Database.ConnectionString,
					useTransaction);

				attempt = await RunMigrationAsync(context, dbContext, useTransaction: false);
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

					await dbContext.CommitTransactionAsync();
					return NextCommand.Continues;
				default:
					throw new InvalidOperationException($"'{attempt.Result}' is an unknown migration result type.");
			}
		}

		private async Task<MigrationLog.ApplyAttempt> RunMigrationAsync(MongoMigrationContext context,
			AdaptiveMongoDbContext dbContext , bool useTransaction)
		{
			//AdaptiveMongoDbContext dbContext = GetMongoDbContext(
			//	context.Database.ConnectionString, 
			//	useTransaction);

			var sw = new Stopwatch();
			var startTime = DateTime.UtcNow;
			MigrationResultType? result = null;
			MigrationLog.ErrorContext errorContext = null;

			try
			{
				MongoMigrationContext migrationContext = context.ForMigration(
					   _migration.Version,
					   dbContext);

				dbContext.StartTransaction();
				sw.Start();

				await _migration.ApplyAsync(migrationContext);

//				await dbContext.CommitTransactionAsync();
				result = MigrationResultType.Completed;
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Migration failed.");

				await dbContext.AbortTransactionAsync();
				
				result = MigrationResultType.Error;
				errorContext = CreateErrorContext(ex);
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
				Error = errorContext
			};
		}

		private static MigrationLog.ErrorContext CreateErrorContext<TException>(TException exception)
			where TException : Exception
		{
			string message = null;
			string stackTrace = exception.ToStringDemystified();
			switch (exception)
			{
				// connection
				//case MongoAuthenticationException _:
				//case MongoConnectionClosedException _:
				//case MongoConnectionException _:
				//	message = exception.Message;
				//	break;
				//// query
				////case MongoCursorNotFoundException _:
				//case MongoQueryException ex:
				//	message = $"Error executing query: {ex.Query}";
				//	break;
				//// cmd
				//case MongoDuplicateKeyException ex:
				//case MongoWriteConcernException ex:
				//case MongoNodeIsRecoveringException ex:
				//case MongoNotPrimaryException ex:
				//case MongoCommandException ex:

				////client
				//case MongoWaitQueueFullException ex:
				//case MongoConfigurationException ex:
				//case MongoClientException ex:
				////svr
				//case MongoBulkWriteException ex:
				//case MongoWriteException ex:
				//case MongoExecutionTimeoutException ex:
				//case MongoServerException ex:
				//// internal - bug
				//case MongoInternalException ex:
				// catch all for mongo ex
				case MongoException ex:
					message = $"MongoException: {ex.Message}";
					break;
				default:
					message = exception.Message;
					break;
			}

			return new MigrationLog.ErrorContext
			{
				ExceptionMessage = message,
				Exception = exception,
				Stacktrace = stackTrace
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
				log = migration.CreateLogFor(attempt);
				//log = new MigrationLog(
				//	migration.Version.Version,
				//	migration.Version.YearQuarter,
				//	migration.Description,
				//	new List<MigrationLog.ApplyAttempt> { attempt });
				await collection.InsertOneAsync(log);
			}
		}
	}
}
