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
		public readonly TPipelineContext Context;

		protected Pipeline(
			Stage<TPipelineContext, TStageContext> headStage,
			TPipelineContext context)
		{
			_headStage = headStage ?? throw new ArgumentNullException(nameof(headStage));
			Context = context ?? throw new ArgumentNullException(nameof(context));
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

		protected virtual Func<Pipeline<TPipelineContext, TStageContext>, Task> OnStart { get; }

		protected virtual Func<Pipeline<TPipelineContext, TStageContext>, Task> OnEnd { get; }

		protected virtual Func<Exception, Pipeline<TPipelineContext, TStageContext>, Task> OnError { get; }
	}
}
