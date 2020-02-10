using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class Stage<TPipelineContext>
		where TPipelineContext : MigrationContext
	{
		private Stage<TPipelineContext> _next { get; set; }
		protected readonly TPipelineContext _context;

		protected Stage(TPipelineContext pipelineContext)
		{
			_context = pipelineContext;
		}

		protected abstract Task<NextCommand> ProcessAsync(TPipelineContext context, object input);

		protected virtual Action<TPipelineContext> OnStart { get; }

		internal async Task ProcessInternalAsync(object input)
		{
			OnStart?.Invoke(_context);

			var next = await ProcessAsync(_context, input);

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
		public Stage<TPipelineContext> SetNext(Stage<TPipelineContext> next)
		{
			return _next = next ?? throw new ArgumentNullException(nameof(next), "Next stage must be provided.");
		}
	}
}
