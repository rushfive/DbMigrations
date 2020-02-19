//using MongoDB.Driver;
//using R5.DbMigrations.Domain;
//using R5.DbMigrations.Mongo.Database;
//using R5.DbMigrations.Mongo.Migrations;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace R5.DbMigrations.Mongo.Processing
//{
//	internal class MongoMigrationPipelineResolver
//	{
//		private readonly MongoMigrationOptions _options;
//		private readonly VersionedDatabase _database;

//		internal MongoMigrationPipelineResolver(
//			MongoMigrationOptions options,
//			VersionedDatabase database)
//		{
//			_options = options;
//			_database = database;
//		}

//		internal MongoMigrationPipeline CreateFor(MongoMigration migration, string connectionString)
//		{
//			MongoMigrationStage headStage = BuildStages(migration);

//			//AdaptiveMongoDbContext db = GetAdaptiveMongoDatabase(connectionString);
//			var context = new MongoMigrationContext(_options, _database, migration.Version);

//			return new MongoMigrationPipeline(headStage, context, _options);
//		}

//		public AdaptiveMongoDbContext GetAdaptiveMongoDatabase(string connectionString)
//			=> _options.UseTransaction
//				? AdaptiveMongoDbContext.WithTransaction(GetMongoDatabase(connectionString))
//				: AdaptiveMongoDbContext.WithoutTransaction(GetMongoDatabase(connectionString));

//		private static IMongoDatabase GetMongoDatabase(string connectionString)
//		{
//			var url = MongoUrl.Create(connectionString);
//			var clientSettings = MongoClientSettings.FromUrl(url);
//			var client = new MongoClient(clientSettings);

//			// todo: configure this too
//			var dbSettings = new MongoDatabaseSettings();
//			var database = client.GetDatabase(url.DatabaseName, dbSettings);
//			return database;
//		}

//		private MongoMigrationStage BuildStages(MongoMigration migration)
//		{

//		}
//	}
//}
