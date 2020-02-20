using System;
using System.Threading.Tasks;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class Stage<TPipelineContext>
		where TPipelineContext : MigrationContext
	{
		protected virtual Func<TPipelineContext, Task> OnStart { get; }

		protected abstract Task<NextCommand> ProcessAsync(TPipelineContext context);

		internal async Task<NextCommand> ProcessInternal(TPipelineContext context)
		{
			if (OnStart != null) await OnStart(context);
			return await ProcessAsync(context);
		}
	}
}
