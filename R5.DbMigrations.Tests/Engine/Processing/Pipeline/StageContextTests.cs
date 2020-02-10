//using R5.DbMigrations.Domain.Versioning;
//using R5.DbMigrations.Engine.Processing;
//using R5.DbMigrations.Tests.Engine.Processing.TestObjects;
//using R5.DbMigrations.Utilities;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace R5.DbMigrations.Tests.Engine.Processing.Pipeline
//{
//	public class StageContextTests : ProcessingTests
//	{
//		[Fact]
//		public async Task LifetimeIsScopedPerStage()
//		{
//			TStageContext compareContext = null;

//			_builder
//				.AddNext(
//					_context,
//					(c, o) =>
//					{
//						Assert.NotSame(compareContext, c);
//						compareContext = c;
//						return NextCommand.Continues.AsAwaitable();
//					})
//				.AddNext(
//					_context,
//					(c, o) =>
//					{
//						Assert.NotSame(compareContext, c);
//						compareContext = c;
//						return NextCommand.Continues.AsAwaitable();
//					})
//				.AddNext(
//					_context,
//					(c, o) =>
//					{
//						Assert.NotSame(compareContext, c);
//						compareContext = c;
//						return NextCommand.Continues.AsAwaitable();
//					});

//			await _builder.Build(_context).RunAsync();
//		}
//	}
//}
