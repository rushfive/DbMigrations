using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine;
using R5.DbMigrations.Engine.Processing;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Tests.Engine.Processing.TestObjects
{
	// pipelineContext lifetime = entire processing of pipeline
	// whereas migrationContext lifetime is scoped to the stage
	public class TPipelineContext : PipelineContext
	{
		public bool SetTrueDuringProcessing { get; set; }
		public int Integer { get; set; } = 10;

		public TPipelineContext(DbVersion version)
			: base(version)
		{

		}
	}
}
