//using R5.DbMigrations.Engine.Processing;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace R5.DbMigrations.Tests.Engine.Processing.TestObjects
//{
//	public class TPipeline : Pipeline<TPipelineContext>
//	{
//		public TPipeline(
//			Stage<TPipelineContext> headStage,
//			TPipelineContext context,
//			Func<Task> onStart = null,
//			Func<Task> onEnd = null,
//			Func<Exception, Task> onError = null)
//			: base(headStage, context)
//		{
//			OnStart = onStart;
//			OnEnd = onEnd;
//			OnError = onError;
//		}

//		protected override Func<Task> OnStart { get; }
//		protected override Func<Task> OnEnd { get; }
//		protected override Func<Exception, Task> OnError { get; }
//	}




//}
