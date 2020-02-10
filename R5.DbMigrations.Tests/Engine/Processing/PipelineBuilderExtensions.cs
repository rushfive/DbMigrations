using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Tests.Engine.Processing.TestObjects;
using R5.DbMigrations.Utilities;
using System;
using System.Threading.Tasks;
using P = R5.DbMigrations.Tests.Engine.Processing.TestObjects.TPipeline;
using PC = R5.DbMigrations.Tests.Engine.Processing.TestObjects.TPipelineContext;

namespace R5.DbMigrations.Tests.Engine.Processing
{
	public static class PipelineBuilderExtensions
	{
		public static PipelineBuilder<P, PC> AddNext(
			this PipelineBuilder<P, PC> builder,
			PC context,
			Func<PC, object, Task<NextCommand>> process,
			Action<Stage<TPipelineContext>> onStart = null)
		{
			return builder.AddNext(
				new TStage(process, context, onStart));
		}
	}
}
