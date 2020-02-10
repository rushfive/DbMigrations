using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using R5.DbMigrations.Domain.Migrations;
using R5.DbMigrations.Mongo.Serializers;

namespace R5.DbMigrations.Mongo
{
	public static class BsonRegistrations
	{
		// should be called by the executing assembly BEFORE
		// interfacing with mongo (to guarantee proper migration-types serialization)
		public static void SetupForMongoMigrations()
		{
			BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
			BsonSerializer.RegisterSerializer(new SemanticVersionSerializer());
			BsonSerializer.RegisterSerializer(new YearQuarterVersionSerializer());

			BsonClassMap.RegisterClassMap<MigrationLog>(cm =>
			{
				cm.AutoMap();
				cm.MapIdProperty(l => l.SemVer).SetSerializer(new SemanticVersionSerializer());
				cm.MapProperty(l => l.Version).SetSerializer(new YearQuarterVersionSerializer());
			});
		}
	}
}
