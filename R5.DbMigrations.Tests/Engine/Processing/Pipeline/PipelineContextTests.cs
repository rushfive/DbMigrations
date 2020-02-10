using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Tests.Engine.Processing.TestObjects;
using R5.DbMigrations.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace R5.DbMigrations.Tests.Engine.Processing.Pipeline
{
	public class PipelineContextTests : ProcessingTests
	{
		[Fact]
		public async Task LifetimeIsScopedPerPipeline()
		{
			_builder
				.AddNext(
					_context,
					(c, o) => NextCommand.Continues.AsAwaitable())
				.AddNext(
					_context,
					(c, o) => NextCommand.Continues.AsAwaitable())
				.AddNext(
					_context,
					(c, o) => NextCommand.Continues.AsAwaitable());

			Func<Pipeline<TPipelineContext, TStageContext>, Task> onPipelineStart = p =>
			{
				Assert.Same(_context, p.Context);
				return Task.CompletedTask;
			};

			Func<Pipeline<TPipelineContext, TStageContext>, Task> onPipelineEnd = p =>
			{
				Assert.Same(_context, p.Context);
				return Task.CompletedTask;
			};

			await _builder
				.Build(
					_context,
					onStart: onPipelineStart,
					onEnd: onPipelineEnd)
				.RunAsync();
		}


	}
}
