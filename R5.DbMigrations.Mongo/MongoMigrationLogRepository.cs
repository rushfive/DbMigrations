using MongoDB.Driver;
using R5.DbMigrations.Domain;
using R5.DbMigrations.Domain.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo
{
	public class MongoMigrationLogRepository : IMigrationLogRepository<MongoMigration>
	{
		private readonly IMongoDatabase _database;

		public MongoMigrationLogRepository(IMongoDatabase database)
		{
			_database = database;
		}

		public async Task SaveAppliedAttemptAsync(MigrationLog.ApplyAttempt attempt, MongoMigration migration)
		{
			var filter = MongoFilters.MigrationLog.SingleMatchingVersion(migration.Version);
			var collection = _database.GetCollection<MigrationLog>(MigrationLog.CollectionName);

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
