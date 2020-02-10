using R5.DbMigrations.Domain.Versioning;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class PipelineContext
	{
		public readonly DbVersion MigrationVersion;

		protected PipelineContext(DbVersion version)
		{
			MigrationVersion = version;
		}
	}
}
