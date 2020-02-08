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
	public class RunsToCompletion
	{
		private readonly TPipelineContext _context;
		private readonly TStageContextResolver _resolver;
		private readonly PipelineBuilder<TPipeline, TPipelineContext, TStageContext> _builder;

		public RunsToCompletion()
		{
			var version = new DbVersion("2020.1.1", "1.1.1");
			_resolver = new TStageContextResolver();
			_context = new TPipelineContext(version, _resolver);
			_builder = PipelineBuilder<TPipeline, TPipelineContext, TStageContext>
				.StartsWith(
					new TStage(
						(c, o) =>
						{
							_context.SetTrueDuringProcessing = true;
							return Task.FromResult<NextCommand>(NextCommand.Continues);
						},
						_context));
		}

		[Fact]
		public async Task SingleStage()
		{
			Assert.False(_context.SetTrueDuringProcessing);

			var p = _builder.Build(_context);
			await p.RunAsync();

			Assert.True(_context.SetTrueDuringProcessing);
		}

		[Fact]
		public async Task MultipleStages()
		{
			_builder.AddNext(new TStage(
				(c, o) =>
				{
					Assert.True(_context.SetTrueDuringProcessing);
					_context.SetTrueDuringProcessing = false;
					return Task.FromResult<NextCommand>(NextCommand.Continues);
				},
				_context));

			var p = _builder.Build(_context);

			Assert.False(_context.SetTrueDuringProcessing);
			await p.RunAsync();
			Assert.False(_context.SetTrueDuringProcessing);
		}
	}
}
