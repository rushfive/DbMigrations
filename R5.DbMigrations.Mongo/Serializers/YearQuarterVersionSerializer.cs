using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using R5.DbMigrations.Domain.Versioning;
using System;

namespace R5.DbMigrations.Mongo.Serializers
{
	public class YearQuarterVersionSerializer : IBsonSerializer<YearQuarterVersion>
	{
		public Type ValueType => typeof(YearQuarterVersion);

		public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, YearQuarterVersion value)
		{
			if (value == null)
				throw new InvalidOperationException("Can't serialize a null YearQuarterVersion.");

			context.Writer.WriteString(value.ToString());
		}

		public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
		{
			var version = value as YearQuarterVersion;
			Serialize(context, args, version);
		}

		public YearQuarterVersion Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			if (context.Reader.CurrentBsonType != BsonType.String)
				throw new InvalidOperationException("Can't deserialize a BSON value into a YearQuarterVersion unless it's a string.");

			var serialized = context.Reader.ReadString();
			return YearQuarterVersion.Parse(serialized);
		}

		object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			if (context.Reader.CurrentBsonType != BsonType.String)
				throw new InvalidOperationException("Can't deserialize a BSON value into a YearQuarterVersion unless it's a string.");

			var serialized = context.Reader.ReadString();
			return YearQuarterVersion.Parse(serialized);
		}
	}
}
