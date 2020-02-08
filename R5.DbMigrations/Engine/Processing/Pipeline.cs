using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class Pipeline<TPipelineContext, TStageContext>
		where TPipelineContext : PipelineContext<TStageContext>
	{
		private readonly Stage<TPipelineContext, TStageContext> _headStage;
		private readonly TPipelineContext _context;

		public Pipeline(
			Stage<TPipelineContext, TStageContext> headStage,
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

		protected virtual Func<Pipeline<TPipelineContext, TStageContext>, Task> OnStart { get; set; } = p =>
		{
			Console.WriteLine($"On STARTING callback invoked for pipeline '{p.GetType().Name}' (version {p._context.MigrationVersion}) !!!!");
			return Task.CompletedTask;
		};

		protected virtual Func<Pipeline<TPipelineContext, TStageContext>, Task> OnEnd { get; set; } = p =>
		{
			Console.WriteLine($"On END callback invoked for pipeline '{p.GetType().Name}' !!!!");
			return Task.CompletedTask;
		};

		protected virtual Func<Exception, Pipeline<TPipelineContext, TStageContext>, Task> OnError { get; set; } = (e, p) =>
		{
			Console.WriteLine($"On ERROR callback invoked for pipeline '{p.GetType().Name}' because "
				+ $"exception: '{e.Message}'");
			return Task.CompletedTask;
		};
	}
}
