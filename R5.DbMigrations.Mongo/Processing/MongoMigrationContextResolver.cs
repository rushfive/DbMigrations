//using MongoDB.Driver;
//using R5.DbMigrations.Engine;
//using R5.DbMigrations.Mongo.Database;
//using R5.DbMigrations.Mongo.Migrations;

//namespace R5.DbMigrations.Mongo.Processing
//{
//	public class MongoMigrationContextResolver// : IMigrationContextResolver<MongoMigrationContext>
//	{
//		private readonly MongoMigrationOptions _options;
//		private readonly string _connectionString;

//		public MongoMigrationContextResolver(
//			MongoMigrationOptions options,
//			string connectionString)
//		{
//			_options = options;
//			_connectionString = connectionString;
//		}


//		public MongoMigrationContext Get()
//		{
//			//var mongoDb = _options.UseTransaction
//			//	? AdaptiveMongoDatabase.WithTransaction(GetMongoDatabase(_connectionString))
//			//	: AdaptiveMongoDatabase.WithoutTransaction(GetMongoDatabase(_connectionString));

//			//return new MongoMigrationContext
//			//{
//			//	Database = mongoDb
//			//};
//			return new MongoMigrationContext();
//		}

//		private static IMongoDatabase GetMongoDatabase(string connectionStr)
//		{
//			var url = MongoUrl.Create(connectionStr);
//			var clientSettings = MongoClientSettings.FromUrl(url);
//			var client = new MongoClient(clientSettings);

//			// todo: configure this too
//			var dbSettings = new MongoDatabaseSettings();
//			var database = client.GetDatabase(url.DatabaseName, dbSettings);
//			return database;
//		}
//	}
//}
