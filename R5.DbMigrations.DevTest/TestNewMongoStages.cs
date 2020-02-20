using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using R5.DbMigrations.Domain;
using R5.DbMigrations.Domain.Migrations;
using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using R5.DbMigrations.Mongo.Processing;
using R5.DbMigrations.Mongo.Processing.Stages;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.DevTest
{
	public class TestNewMongoStages
	{
		private readonly MongoMigrationOptions _options = new MongoMigrationOptions
		{
			UseTransaction = true,
			RetryWithoutTransactionOnFail = true
		};

		private readonly Microsoft.Extensions.Logging.ILogger _logger;

		public TestNewMongoStages(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger("test");
		}

		public static TestNewMongoStages Initialize()
		{
			var services = new ServiceCollection();
			services.AddLogging();
			services.AddScoped<TestNewMongoStages>();

			// Initialize Autofac
			var builder = new ContainerBuilder();
			// Use the Populate method to register services which were registered
			// to IServiceCollection
			builder.Populate(services);

			// Build the final container
			IContainer container = builder.Build();

			var loggerFactory = container.Resolve<ILoggerFactory>();
			loggerFactory//.AddConsole()
				.AddSerilog();

			var logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				//.WriteTo.File(new RenderedCompactJsonFormatter(), "TestLogs.txt")
				.CreateLogger();
			Log.Logger = logger;

			return container.Resolve<TestNewMongoStages>();
		}

		public async Task ExecuteTestAsync(List<MongoMigration> mongoMigrations)
		{
			VersionedDatabase versionedDb = await GetTestVersionedDb();

			List<MongoMigration> requiredMigrations = versionedDb.GetRequiredMigrations<MongoMigration, MongoMigrationContext>(mongoMigrations);
			//requiredMigrations = mongoMigrations.Skip(1).ToList();//
			var context = MongoMigrationContext.Initialize(_options, versionedDb);
			List<MongoMigrationStage> stages = GetStages(requiredMigrations);

			var pipeline = new MongoMigrationPipeline(stages, context);
			await pipeline.RunAsync();
		}

		public List<MongoMigrationStage> GetStages(List<MongoMigration> migrations)
		{
			return migrations.Select(m => new RunMigrationStage(_logger, m) as MongoMigrationStage).ToList();
		}

		private async Task<VersionedDatabase> GetTestVersionedDb()
		{
			string connectionStr = "mongodb://mongo1:9560,mongo2:9561/DbUpgradeEval?replicaSet=dockerdev";
			var db = GetMongoDatabase(connectionStr);
			//var fo = new FindOptions<MigrationLog>
			//{
			//	Projection = Builders<MigrationLog>.Projection.Exclude(l => l.H)
			//}
			var migrationsCursor = await db.GetCollection<MigrationLog>(MigrationLog.CollectionName)
				.FindAsync(FilterDefinition<MigrationLog>.Empty);
			var migrations = await migrationsCursor.ToListAsync();

			return new VersionedDatabase("Test Migrations DB", connectionStr, migrations);
		}

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
	}


	public class MigrationTest_1 : MongoMigration
	{
		public override DbVersion Version => DbVersion.NewDatabase;
		public override string Description => "Initial migration";
		public override bool CanUseTransaction => false;

		public override async Task ApplyAsync(MongoMigrationContext context)
		{
			await context.DbContext.CreateCollectionAsync("TestCollection_1");
			await context.DbContext.CreateCollectionAsync("TestCollection_2");
			await context.DbContext.CreateCollectionAsync("TestCollection_3");
		}
	}

	public class MigrationTest_2 : MongoMigration
	{
		public override DbVersion Version => new DbVersion("2020.1.0", "1.0.0");
		public override string Description => "First real migration";

		public override async Task ApplyAsync(MongoMigrationContext context)
		{
			var c1 = context.DbContext.GetCollection<BsonDocument>("TestCollection_1");

			await c1.InsertManyAsync(new List<BsonDocument>
			{
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 1" },
					{ "IntValue", 55 }
				},
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 2" },
					{ "IntValue", 43210 }
				}
			});
		}
	}

	public class MigrationTest_3 : MongoMigration
	{
		public override DbVersion Version => new DbVersion("2020.1.1", "1.0.1");
		public override string Description => "2nd migration";

		public override async Task ApplyAsync(MongoMigrationContext context)
		{
			var c1 = context.DbContext.GetCollection<BsonDocument>("TestCollection_2");

			await c1.InsertManyAsync(new List<BsonDocument>
			{
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 1" },
					{ "IntValue", 55 }
				},
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 2" },
					{ "IntValue", 43210 }
				}
			});

			await c1.InsertManyAsync(new List<BsonDocument>
			{
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 3" },
					{ "IntValue", 333333333 }
				},
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 4" },
					{ "IntValue", 4444444444 }
				}
			});
		}
	}

	public class MigrationTest_4 : MongoMigration
	{
		public override DbVersion Version => new DbVersion("2020.1.2", "1.0.2");
		public override string Description => "3rd migration";

		public override async Task ApplyAsync(MongoMigrationContext context)
		{
			var c1 = context.DbContext.GetCollection<BsonDocument>("TestCollection_3");

			await c1.InsertManyAsync(new List<BsonDocument>
			{
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 1" },
					{ "IntValue", 55 }
				},
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 2" },
					{ "IntValue", 43210 }
				}
			});
			//throw new InvalidOperationException("FAILING migration 4!!!");
			await c1.InsertManyAsync(new List<BsonDocument>
			{
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 3" },
					{ "IntValue", 333333333 }
				},
				new BsonDocument
				{
					{ "_id", Guid.NewGuid().ToString() },
					{ "StringValue", "String value 4" },
					{ "IntValue", 4444444444 }
				}
			});
		}
	}
}
