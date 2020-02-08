using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class PipelineContext<TMigrationContext>
	{
		public readonly DbVersion MigrationVersion;
		public readonly IStageContextResolver<TMigrationContext> StageContextResolver;

		protected PipelineContext(
			DbVersion version,
			IStageContextResolver<TMigrationContext> stageContextResolver)
		{
			MigrationVersion = version;
			StageContextResolver = stageContextResolver;
		}
	}
}
