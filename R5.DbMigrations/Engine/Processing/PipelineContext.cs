using R5.DbMigrations.Domain.Versioning;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class PipelineContext<TStageContext>
	{
		public readonly DbVersion MigrationVersion;
		public readonly IMigrationContextResolver<TStageContext> StageContextResolver;

		protected PipelineContext(
			DbVersion version,
			IMigrationContextResolver<TStageContext> stageContextResolver)
		{
			MigrationVersion = version;
			StageContextResolver = stageContextResolver;
		}
	}
}
