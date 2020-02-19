using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class Stage<TPipelineContext>
		where TPipelineContext : MigrationContext
	{
		//private Stage<TPipelineContext> _next { get; set; }
		//protected readonly TPipelineContext _context;

		//protected Stage(TPipelineContext pipelineContext)
		//{
		//	_context = pipelineContext;
		//}
		//protected Stage() { }

		protected virtual Func<TPipelineContext, Task> OnStart { get; }
		//protected virtual Func<TPipelineContext, Task> OnComplete { get; }
		//protected virtual Func<TPipelineContext, Task> OnError { get; }

		protected abstract Task<NextCommand> ProcessAsync(TPipelineContext context);

		internal async Task<NextCommand> ProcessInternal(TPipelineContext context)
		{
			if (OnStart != null) await OnStart(context);
			return await ProcessAsync(context);
		}

		//internal async Task<NextCommand> ProcessInternalAsync(TPipelineContext context)
		//{
		//	try
		//	{
		//		if (OnStart != null) await OnStart(context);

		//		var next = await ProcessAsync(context);

		//		if (OnComplete != null) await OnComplete(context);
		//		return next;
		//	}
		//	catch (Exception ex)
		//	{
		//		if (OnError != null) await OnError(context);
		//		throw;
		//	}
		//}

		// links current to the next stage, then returns next
		//public Stage<TPipelineContext> SetNext(Stage<TPipelineContext> next)
		//{
		//	return _next = next ?? throw new ArgumentNullException(nameof(next), "Next stage must be provided.");
		//}
	}
}
