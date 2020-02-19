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
//	public class RunsToCompletionTests : ProcessingTests
//	{
//		[Fact]
//		public async Task SingleStage()
//		{
//			Assert.False(_context.SetTrueDuringProcessing);

//			_builder.AddNext(
//				_context,
//				(c, o) =>
//				{
//					_context.SetTrueDuringProcessing = true;
//					return NextCommand.Continues.AsAwaitable();
//				});

//			await _builder.Build(_context).RunAsync();
//			Assert.True(_context.SetTrueDuringProcessing);
//		}

//		[Fact]
//		public async Task MultipleStages()
//		{
//			_builder
//				.AddNext(
//					_context,
//					(c, o) =>
//					{
//						_context.SetTrueDuringProcessing = true;
//						return NextCommand.Continues.AsAwaitable();
//					})
//				.AddNext(
//					_context,
//					(c, o) =>
//					{
//						Assert.True(_context.SetTrueDuringProcessing);
//						_context.SetTrueDuringProcessing = false;
//						return NextCommand.Continues.AsAwaitable();
//					});

//			Assert.False(_context.SetTrueDuringProcessing);
//			await _builder.Build(_context).RunAsync();
//			Assert.False(_context.SetTrueDuringProcessing);
//		}
//	}
//}
