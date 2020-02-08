using R5.DbMigrations.Domain.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using R5.DbMigrations.Domain.Versioning;

namespace R5.DbMigrations.Domain.Processing
{
	// options should be used strictly to build the pipeline
	// stages.

	public abstract class Pipeline<TPipelineContext, TMigrationContext>
		where TPipelineContext : PipelineContext<TMigrationContext>
	{
		private readonly Stage<TPipelineContext, TMigrationContext> _headStage;
		private readonly TPipelineContext _context;

		public Pipeline(
			Stage<TPipelineContext, TMigrationContext> headStage,
			TPipelineContext context)
		{
			_headStage = headStage ?? throw new ArgumentNullException(nameof(headStage));
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task RunAsync()
		{
			try
			{
				if (OnStart != null) await OnStart(this);
				await _headStage.ProcessInternalAsync(default);
			}
			catch (Exception ex)
			{
				if (OnError != null) await OnError(ex, this);
				throw;
			}
			finally
			{
				if (OnEnd != null) await OnEnd(this);
			}
		}

		protected virtual Func<Pipeline<TPipelineContext, TMigrationContext>, Task> OnStart { get; set; } = p =>
		{
			Console.WriteLine($"On STARTING callback invoked for pipeline '{p.GetType().Name}' (version {p._context.MigrationVersion}) !!!!");
			return Task.CompletedTask;
		};

		protected virtual Func<Pipeline<TPipelineContext, TMigrationContext>, Task> OnEnd { get; set; } = p =>
		{
			Console.WriteLine($"On END callback invoked for pipeline '{p.GetType().Name}' !!!!");
			return Task.CompletedTask;
		};

		protected virtual Func<Exception, Pipeline<TPipelineContext, TMigrationContext>, Task> OnError { get; set; } = (e, p) =>
		{
			Console.WriteLine($"On ERROR callback invoked for pipeline '{p.GetType().Name}' because "
				+ $"exception: '{e.Message}'");
			return Task.CompletedTask;
		};
	}

	public abstract class PipelineContext<TMigrationContext>
	{
		// pipeline PER migration
		public DbVersion MigrationVersion { get; set; }
		public IMigrationContextResolver<TMigrationContext> MigrationContextResolver { get; set; }
	}


	public abstract class Stage<TPipelineContext, TMigrationContext>
		where TPipelineContext : PipelineContext<TMigrationContext>
	{
		private Stage<TPipelineContext, TMigrationContext> _next { get; set; }
		private readonly TPipelineContext _pipelineContext;

		protected Stage(TPipelineContext pipelineContext)
		{
			_pipelineContext = pipelineContext;
		}

		protected abstract Task<NextCommand> ProcessAsync(TMigrationContext context, object input);

		protected virtual Action<Stage<TPipelineContext, TMigrationContext>> OnStart { get; set; } = stage =>
		{
			Console.WriteLine($"OnStart callback invoked for stage '{stage.GetType().Name}");
		};

		internal async Task ProcessInternalAsync(object input)
		{
			OnStart?.Invoke(this);

			var context = _pipelineContext.MigrationContextResolver.Get();
			var next = await ProcessAsync(context, input);
			switch (next)
			{
				case NextCommand.Continue _:
					await _next.ProcessInternalAsync(null);
					break;
				case NextCommand.ContinueWith cmd:
					await _next.ProcessInternalAsync(cmd.Result);
					break;
				case NextCommand.End _:
					return;
				default:
					throw new InvalidOperationException($"'{next.GetType().Name}' is an invalid NextCommand specifier type.");
			}
		}

		// links current to the next stage, then returns next
		public Stage<TPipelineContext, TMigrationContext> SetNext(Stage<TPipelineContext, TMigrationContext> next)
		{
			return _next = next ?? throw new ArgumentNullException(nameof(next), "Next stage must be provided.");
		}
	}

	public abstract class NextCommand
	{
		public class Continue : NextCommand { }

		public class ContinueWith : NextCommand
		{
			public readonly object Result;
			public ContinueWith(object result) => Result = result;
		}

		public class End : NextCommand { }
	}

	public static class Pipeline
	{
		public static readonly NextCommand.Continue Continues = new NextCommand.Continue();
		public static NextCommand.ContinueWith ContinuesWith(object result) => new NextCommand.ContinueWith(result);
		public static readonly NextCommand.End Ends = new NextCommand.End();
	}
}
