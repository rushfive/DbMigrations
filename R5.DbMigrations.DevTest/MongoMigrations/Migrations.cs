//using MongoDB.Bson;
//using MongoDB.Driver;
//using R5.DbMigrations.Domain.Versioning;
//using R5.DbMigrations.Mongo.Migrations;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace R5.DbMigrations.DevTest.MongoMigrations
//{
//	public class ThirdUpgrade_FAILS : MongoMigration
//	{
//		public override DbVersion Version
//			=> new DbVersion("2020.1.2", "1.0.2");

//		public override string Description => "ThirdUpgrade - this one fails!";

//		public override async Task ApplyAsync(object migrationContext)
//		{
//			MongoMigrationContext context = MapContext(migrationContext);

//			var collection1 = context.DbContext.GetCollection<BsonDocument>("TestCollection2");
//			//throw new Exception("This migration will fail hahahaha!");
//			await collection1.UpdateManyAsync(
//				FilterDefinition<BsonDocument>.Empty,
//				Builders<BsonDocument>.Update.Set(d => d["ThirdUpgrade_ShouldNOTSEETHIS"], "3rd upgrade setr to fail -> SHOULD NOT SEE THIS!!!!"));

//		}
//	}

//	public class FourthUpgrade : MongoMigration
//	{
//		public override DbVersion Version
//			=> new DbVersion("2020.1.3", "1.0.3");

//		public override string Description => "FourthUpgrade";

//		public override async Task ApplyAsync(object migrationContext)
//		{
//			MongoMigrationContext context = MapContext(migrationContext);

//			var collection1 = context.DbContext.GetCollection<BsonDocument>("TestCollection2");

//			await collection1.UpdateManyAsync(
//				FilterDefinition<BsonDocument>.Empty,
//				Builders<BsonDocument>.Update.Set(d => d["FourthUpgrade"], "added from fourth upgrade!"));

//		}
//	}
//	public class Initial : MongoMigration
//	{
//		public override DbVersion Version => DbVersion.NewDatabase;

//		public override string Description => "initial";

//		public override async Task ApplyAsync(object migrationContext)
//		{
//			MongoMigrationContext context = MapContext(migrationContext);

//			await context.DbContext.CreateCollectionAsync("TestCollection1");
//			await context.DbContext.CreateCollectionAsync("TestCollection2");

//			var collection1 = context.DbContext.GetCollection<BsonDocument>("TestCollection1");
//			var collection2 = context.DbContext.GetCollection<BsonDocument>("TestCollection2");

//			await collection1.InsertManyAsync(new List<BsonDocument>
//			{
//				new BsonDocument
//				{
//					{ "_id", Guid.NewGuid().ToString() },
//					{ "text", "collection 1 item 1" }
//				},
//				new BsonDocument
//				{
//					{ "_id", Guid.NewGuid().ToString() },
//					{ "text", "collection 1 item 2" }
//				}
//			});
//			await collection2.InsertManyAsync(new List<BsonDocument>
//			{
//				new BsonDocument
//				{
//					{ "_id", Guid.NewGuid().ToString() },
//					{ "text", "collection 2 item 1" }
//				},
//				new BsonDocument
//				{
//					{ "_id", Guid.NewGuid().ToString() },
//					{ "text", "collection 2 item 2" }
//				}
//			});
//		}
//	}

//	public class FirstUpgrade : MongoMigration
//	{
//		public override DbVersion Version
//			=> new DbVersion("2020.1.0", "1.0.0");

//		public override string Description => "FirstUpgrade";

//		public override async Task ApplyAsync(object migrationContext)
//		{
//			MongoMigrationContext context = MapContext(migrationContext);

//			var collection1 = context.DbContext.GetCollection<BsonDocument>("TestCollection1");

//			await collection1.UpdateManyAsync(
//				FilterDefinition<BsonDocument>.Empty,
//				Builders<BsonDocument>.Update.Set(d => d["FirstUpgrade_Property"], "added from first upgrade"));

//		}
//	}

//	public class SecondUpgrade : MongoMigration
//	{
//		public override DbVersion Version
//			=> new DbVersion("2020.1.1", "1.0.1");

//		public override string Description => "SecondUpgrade";

//		public override async Task ApplyAsync(object migrationContext)
//		{
//			MongoMigrationContext context = MapContext(migrationContext);

//			var collection1 = context.DbContext.GetCollection<BsonDocument>("TestCollection2");

//			await collection1.UpdateManyAsync(
//				FilterDefinition<BsonDocument>.Empty,
//				Builders<BsonDocument>.Update.Set(d => d["SecondUpgrade_Property"], "added from second upgrade"));

//		}
//	}

//	public class DifferentSetUpgrade : MongoMigration
//	{
//		public override DbVersion Version
//			=> new DbVersion("2020.1.2", "1.0.2");

//		public override string Description => "Differnet set upgrade";

//		public override async Task ApplyAsync(object migrationContext)
//		{
//			MongoMigrationContext context = MapContext(migrationContext);

//			var collection1 = context.DbContext.GetCollection<BsonDocument>("TestCollection2");

//			await collection1.UpdateManyAsync(
//				FilterDefinition<BsonDocument>.Empty,
//				Builders<BsonDocument>.Update.Set(d => d["OtherSetUpgraded"], "updated via the other set upgr"));
//		}
//	}

//	//public class ThirdUpgrade_FAILS : MongoMigration
//	//{
//	//	public override DbVersion Version
//	//		=> new DbVersion("2020.1.2", "1.0.2");

//	//	public override string Description => "ThirdUpgrade - this one fails!";

//	//	public override async Task ApplyAsync(object migrationContext)
//	//	{
//	//		MongoMigrationContext context = MapContext(migrationContext);

//	//		var collection1 = context.Database.GetCollection<BsonDocument>("TestCollection2");
//	//		//throw new Exception("This migration will fail hahahaha!");
//	//		await collection1.UpdateManyAsync(
//	//			FilterDefinition<BsonDocument>.Empty,
//	//			Builders<BsonDocument>.Update.Set(d => d["ThirdUpgrade_ShouldNOTSEETHIS"], "3rd upgrade setr to fail -> SHOULD NOT SEE THIS!!!!"));

//	//	}
//	//}

//	//public class FourthUpgrade : MongoMigration
//	//{
//	//	public override DbVersion Version
//	//		=> new DbVersion("2020.1.3", "1.0.3");

//	//	public override string Description => "FourthUpgrade";

//	//	public override async Task ApplyAsync(object migrationContext)
//	//	{
//	//		MongoMigrationContext context = MapContext(migrationContext);

//	//		var collection1 = context.Database.GetCollection<BsonDocument>("TestCollection2");

//	//		await collection1.UpdateManyAsync(
//	//			FilterDefinition<BsonDocument>.Empty,
//	//			Builders<BsonDocument>.Update.Set(d => d["FourthUpgrade"], "added from fourth upgrade!"));

//	//	}
//	//}

//	public class FithUpgrade : MongoMigration
//	{
//		public override DbVersion Version
//			=> new DbVersion("2020.1.4", "1.0.4");

//		public override string Description => "FifthUpgrade";

//		public override async Task ApplyAsync(object migrationContext)
//		{
//			MongoMigrationContext context = MapContext(migrationContext);

//			var collection1 = context.DbContext.GetCollection<BsonDocument>("TestCollection1");

//			await collection1.UpdateManyAsync(
//				FilterDefinition<BsonDocument>.Empty,
//				Builders<BsonDocument>.Update.Set(d => d["FifthUpgrade"], "test transaction!"));

//		}
//	}

//	public class SixthUpgrade : MongoMigration
//	{
//		public override DbVersion Version
//			=> new DbVersion("2020.1.5", "1.0.5");

//		public override string Description => "SixthUpgrade";

//		public override async Task ApplyAsync(object migrationContext)
//		{
//			MongoMigrationContext context = MapContext(migrationContext);

//			var collection1 = context.DbContext.GetCollection<BsonDocument>("TestCollection1");

//			await collection1.UpdateManyAsync(
//				FilterDefinition<BsonDocument>.Empty,
//				Builders<BsonDocument>.Update.Set(d => d["SixthUpgrade"], "SixthUpgraden!"));

//		}
//	}
//}
