using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Tests.Engine.Processing.TestObjects;
using R5.DbMigrations.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace R5.DbMigrations.Tests.Engine.Processing
{
	// Setup that's shared for processing (eg pipelines, stages) can be set here
	public abstract class ProcessingTests
	{
		protected readonly TPipelineContext _context;
		protected readonly PipelineBuilder<TPipeline, TPipelineContext> _builder;

		protected ProcessingTests()
		{
			var version = new DbVersion("2020.1.1", "1.1.1");
			_context = new TPipelineContext(version);
			_builder = new PipelineBuilder<TPipeline, TPipelineContext>();
		}
	}
}
