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
	public class PipelineEventHookTests : ProcessingTests
	{
		[Fact]
		public async Task OnStart_Set_Invoked()
		{
			_builder.AddNext(
				_context,
				(c, o) => NextCommand.Continues.AsAwaitable());

			Func<TPipelineContext, Task> onPipelineStart = p =>
			{
				_context.Integer++;
				return Task.CompletedTask;
			};

			int preProcessValue = _context.Integer;

			await _builder
				.Build(
					_context,
					onStart: onPipelineStart)
				.RunAsync();

			Assert.NotEqual(preProcessValue, _context.Integer);
		}

		[Fact]
		public async Task OnStart_NotSet_NotInvoked()
		{
			_builder.AddNext(
				_context,
				(c, o) => NextCommand.Continues.AsAwaitable());

			int preProcessValue = _context.Integer;

			await _builder
				.Build(_context)
				.RunAsync();

			Assert.Equal(preProcessValue, _context.Integer);
		}

		[Fact]
		public async Task OnEnd_Set_Invokes_OnPipelineCompletion()
		{
			int preProcessValue = _context.Integer;

			_builder.AddNext(
				_context,
				(c, o) =>
				{
					Assert.Equal(preProcessValue, _context.Integer);
					return NextCommand.Continues.AsAwaitable();
				});

			Func<TPipelineContext, Task> onPipelineEnd = p =>
			{
				Assert.Equal(preProcessValue, _context.Integer);
				_context.Integer++;
				return Task.CompletedTask;
			};

			await _builder
				.Build(
					_context,
					onEnd: onPipelineEnd)
				.RunAsync();

			Assert.NotEqual(preProcessValue, _context.Integer);
		}

		[Fact]
		public async Task OnEnd_Set_Invokes_OnPipelineCompletion_EvenOnError()
		{
			int preProcessValue = _context.Integer;

			_builder.AddNext(
				_context,
				(c, o) => throw new ProcessTestException(default));

			Func<TPipelineContext, Task> onPipelineEnd = p =>
			{
				Assert.Equal(preProcessValue, _context.Integer);
				_context.Integer++;
				return Task.CompletedTask;
			};

			Func<Task> runTest = () => _builder
				.Build(
					_context,
					onEnd: onPipelineEnd)
				.RunAsync();

			await Assert.ThrowsAsync<ProcessTestException>(runTest);

			Assert.NotEqual(preProcessValue, _context.Integer);
		}

		[Fact]
		public async Task OnError_Set_Invoked()
		{
			int preProcessValue = _context.Integer;

			_builder.AddNext(
				_context,
				(c, o) => throw new ProcessTestException(5));

			Func<Exception, TPipelineContext, Task> onPipelineError = (e, p) =>
			{
				var testException = e as ProcessTestException;
				Assert.NotNull(testException);
				Assert.Equal(5, testException.Integer);
				return Task.CompletedTask;
			};

			Func<Task> runTest = () => _builder
				.Build(
					_context,
					onError: onPipelineError)
				.RunAsync();

			await Assert.ThrowsAsync<ProcessTestException>(runTest);
		}

		[Fact]
		public async Task OnError_NotSet_NotInvoked()
		{
			int preProcessValue = _context.Integer;

			_builder.AddNext(
				_context,
				(c, o) => throw new ProcessTestException(5));

			Func<Task> runTest = () => _builder
				.Build(_context)
				.RunAsync();

			await Assert.ThrowsAsync<ProcessTestException>(runTest);

			Assert.Equal(preProcessValue, _context.Integer);
		}
	}
}
