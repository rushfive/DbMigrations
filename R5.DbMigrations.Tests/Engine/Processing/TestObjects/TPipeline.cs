using R5.DbMigrations.Engine.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.DbMigrations.Tests.Engine.Processing.TestObjects
{
	public class TPipeline : Pipeline<TPipelineContext, TStageContext>
	{
		public TPipeline(
			Stage<TPipelineContext, TStageContext> headStage,
			TPipelineContext context)
			: base(headStage, context)
		{

		}
	}

	

	
}
