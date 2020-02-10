using R5.DbMigrations.Domain;
using R5.DbMigrations.Domain.Versioning;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class MigrationContext
	{
		public readonly VersionedDatabase Database;
		public readonly DbVersion MigrationVersion;

		protected MigrationContext(
			VersionedDatabase database,
			DbVersion version)
		{
			Database = database;
			MigrationVersion = version;
		}
	}
}
