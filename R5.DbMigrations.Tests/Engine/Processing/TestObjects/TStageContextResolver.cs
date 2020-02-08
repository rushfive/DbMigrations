using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine;
using R5.DbMigrations.Engine.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Tests.Engine.Processing.TestObjects
{
	public class TStageContextResolver : IStageContextResolver<TStageContext>
	{
		public TStageContext Get()
		{
			return new TStageContext
			{
				Integer = 500
			};
		}
	}

	// pipelineContext lifetime = entire processing of pipeline
	// whereas migrationContext lifetime is scoped to the stage
	public class TPipelineContext : PipelineContext<TStageContext>
	{
		public bool SetTrueDuringProcessing { get; set; }


		public TPipelineContext(
			DbVersion version,
			IStageContextResolver<TStageContext> stageContextResolver)
			: base(version, stageContextResolver)
		{

		}
	}

	public class TStageContext
	{
		public int Integer { get; set; }
	}
}
