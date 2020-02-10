using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using R5.DbMigrations.Domain.Versioning;
using System;

namespace R5.DbMigrations.Mongo.Serializers
{
	public class SemanticVersionSerializer : IBsonSerializer<SemanticVersion>
	{
		public Type ValueType => typeof(SemanticVersion);

		public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, SemanticVersion value)
		{
			if (value == null)
				throw new InvalidOperationException("Can't serialize a null SemanticVersion.");

			context.Writer.WriteString(value.ToString());
		}

		public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
		{
			var version = value as SemanticVersion;
			Serialize(context, args, version);
		}

		public SemanticVersion Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			if (context.Reader.CurrentBsonType != BsonType.String)
				throw new InvalidOperationException("Can't deserialize a BSON value into a SemanticVersion unless it's a string.");

			var serialized = context.Reader.ReadString();
			return SemanticVersion.Parse(serialized);
		}

		object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
		{
			if (context.Reader.CurrentBsonType != BsonType.String)
				throw new InvalidOperationException("Can't deserialize a BSON value into a SemanticVersion unless it's a string.");

			var serialized = context.Reader.ReadString();
			return SemanticVersion.Parse(serialized);
		}
	}
}
