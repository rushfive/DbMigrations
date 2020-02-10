using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class Pipeline<TPipelineContext>
		where TPipelineContext : MigrationContext
	{
		private readonly Stage<TPipelineContext> _headStage;
		protected readonly TPipelineContext _context;

		protected Pipeline(
			Stage<TPipelineContext> headStage,
			TPipelineContext context)
		{
			_headStage = headStage ?? throw new ArgumentNullException(nameof(headStage));
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task RunAsync()
		{
			try
			{
				if (OnStart != null) await OnStart();
				await _headStage.ProcessInternalAsync(default);
			}
			catch (Exception ex)
			{
				if (OnError != null) await OnError(ex);
				throw;
			}
			finally
			{
				if (OnEnd != null) await OnEnd();
			}
		}

		protected virtual Func<Task> OnStart { get; }

		protected virtual Func<Task> OnEnd { get; }

		protected virtual Func<Exception, Task> OnError { get; }
	}
}
