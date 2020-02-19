//using Microsoft.Extensions.Logging;
//using MongoDB.Driver;
//using R5.DbMigrations.Engine.Processing;
//using R5.DbMigrations.Mongo.Database;
//using R5.DbMigrations.Mongo.Migrations;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace R5.DbMigrations.Mongo.Processing.Stages
//{
//	public class ConnectToDbStage : MongoMigrationStage
//	{
//		protected override string Description 
//			=> $"Initialize connection to db '{_context.Database.ConnectionString}'";

//		public ConnectToDbStage(
//			MongoMigrationContext context,
//			ILoggerFactory loggerFactory)
//			: base(context, loggerFactory)
//		{

//		}

//		protected override Task<NextCommand> ProcessAsync(MongoMigrationContext context, object input)
//		{
//			AdaptiveMongoDbContext dbContext = GetMongoDbContext(context.Database.ConnectionString);
//			context.SetDbContext(dbContext);
//			return NextCommand.Continues.AsAwaitable();
//		}

//		public AdaptiveMongoDbContext GetMongoDbContext(string connectionString)
//			=> _context.Options.UseTransaction
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
//	}
//}
