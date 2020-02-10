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
	public class NextCommandEvaluationTests : ProcessingTests
	{
		[Fact]
		public async Task Continue_ProgressesToNextStage()
		{
			int testValue = 5;

			_builder
				.AddNext(
					_context,
					(c, o) =>
					{
						testValue = 10;
						return NextCommand.Continues.AsAwaitable();
					})
				.AddNext(
					_context,
					(c, o) =>
					{
						testValue = 20;
						return NextCommand.Continues.AsAwaitable();
					});

			await _builder.Build(_context).RunAsync();
			Assert.Equal(20, testValue);
		}

		public class TestResultObject
		{
			public readonly Guid Id = Guid.NewGuid();
		}

		[Fact]
		public async Task ContinueWith_ProgressesToNextStage_PassingResult()
		{
			var result = new TestResultObject();

			_builder
				.AddNext(
					_context,
					(c, o) =>
					{
						return NextCommand.ContinuesWith(result).AsAwaitable();
					})
				.AddNext(
					_context,
					(c, o) =>
					{
						Assert.Same(result, o);
						var priorStageResult = o as TestResultObject;
						Assert.NotNull(priorStageResult);
						Assert.Equal(result.Id, priorStageResult.Id);
						return NextCommand.Continues.AsAwaitable();
					});

			await _builder.Build(_context).RunAsync();
		}

		[Fact]
		public async Task End_TerminatesFurtherProcessing()
		{
			int testValue = 5;

			_builder
				.AddNext(
					_context,
					(c, o) =>
					{
						testValue = 10;
						return NextCommand.Ends.AsAwaitable();
					})
				.AddNext(
					_context,
					(c, o) =>
					{
						testValue = 20;
						return NextCommand.Continues.AsAwaitable();
					});

			await _builder.Build(_context).RunAsync();
			Assert.Equal(10, testValue);
		}
	}
}
