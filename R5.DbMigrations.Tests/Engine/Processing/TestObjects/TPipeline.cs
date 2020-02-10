using R5.DbMigrations.Engine.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Tests.Engine.Processing.TestObjects
{
	public class TPipeline : Pipeline<TPipelineContext, TStageContext>
	{
		public TPipeline(
			Stage<TPipelineContext, TStageContext> headStage,
			TPipelineContext context,
			Func<Pipeline<TPipelineContext, TStageContext>, Task> onStart = null,
			Func<Pipeline<TPipelineContext, TStageContext>, Task> onEnd = null,
			Func<Exception, Pipeline<TPipelineContext, TStageContext>, Task> onError = null)
			: base(headStage, context)
		{
			OnStart = onStart;
			OnEnd = onEnd;
			OnError = onError;
		}

		protected override Func<Pipeline<TPipelineContext, TStageContext>, Task> OnStart { get; }
		protected override Func<Pipeline<TPipelineContext, TStageContext>, Task> OnEnd { get; }
		protected override Func<Exception, Pipeline<TPipelineContext, TStageContext>, Task> OnError { get; }
	}




}
