using R5.DbMigrations.Engine.Processing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Tests.Engine.Processing.TestObjects
{
	public class TStage : Stage<TPipelineContext, TStageContext>
	{
		private readonly Func<TStageContext, object, Task<NextCommand>> _process;

		public TStage(
			Func<TStageContext, object, Task<NextCommand>> process,
			TPipelineContext context)
			: base(context)
		{
			_process = process;
		}

		protected override Task<NextCommand> ProcessAsync(TStageContext context, object input)
		{
			return _process(context, input);
		}
	}
}
