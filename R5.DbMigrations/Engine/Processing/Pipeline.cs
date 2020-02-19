using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class Pipeline<TPipelineContext>
		where TPipelineContext : MigrationContext
	{
		private readonly List<Stage<TPipelineContext>> _stages;
		protected readonly TPipelineContext _context;

		protected Pipeline(
			List<Stage<TPipelineContext>> stages,
			TPipelineContext context)
		{
			_stages = stages;
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task RunAsync()
		{
			if (OnStart != null) await OnStart();

			foreach (var s in _stages)
			{
				NextCommand next = await s.ProcessInternal(_context);

				if (next is NextCommand.End)
				{
					break;
				}
			}

			if (OnEnd != null) await OnEnd();
		}

		protected virtual Func<Task> OnStart { get; }
		protected virtual Func<Task> OnEnd { get; }
	}
}
