using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class Stage<TPipelineContext, TStageContext>
		where TPipelineContext : PipelineContext<TStageContext>
	{
		private Stage<TPipelineContext, TStageContext> _next { get; set; }
		private readonly TPipelineContext _pipelineContext;

		protected Stage(TPipelineContext pipelineContext)
		{
			_pipelineContext = pipelineContext;
		}

		protected abstract Task<NextCommand> ProcessAsync(TStageContext context, object input);

		protected virtual Action<Stage<TPipelineContext, TStageContext>> OnStart { get; }

		internal async Task ProcessInternalAsync(object input)
		{
			OnStart?.Invoke(this);

			var context = _pipelineContext.StageContextResolver.Get();
			var next = await ProcessAsync(context, input);

			if (_next != null)
			{
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
		}

		// links current to the next stage, then returns next
		public Stage<TPipelineContext, TStageContext> SetNext(Stage<TPipelineContext, TStageContext> next)
		{
			return _next = next ?? throw new ArgumentNullException(nameof(next), "Next stage must be provided.");
		}
	}
}
