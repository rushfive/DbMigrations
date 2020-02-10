using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Tests.Engine.Processing.TestObjects;
using R5.DbMigrations.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using P = R5.DbMigrations.Tests.Engine.Processing.TestObjects.TPipeline;
using PC = R5.DbMigrations.Tests.Engine.Processing.TestObjects.TPipelineContext;
using SC = R5.DbMigrations.Tests.Engine.Processing.TestObjects.TStageContext;

namespace R5.DbMigrations.Tests.Engine.Processing
{
	public static class PipelineBuilderExtensions
	{
		public static PipelineBuilder<P, PC, SC> AddNext(
			this PipelineBuilder<P, PC, SC> builder,
			PC context,
			Func<SC, object, Task<NextCommand>> process,
			Action<Stage<TPipelineContext, TStageContext>> onStart = null)
		{
			return builder.AddNext(
				new TStage(process, context, onStart));
		}
	}
}
