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
//	public class StageEventHookTests : ProcessingTests
//	{
//		[Fact]
//		public async Task OnStart_Set_Invoked()
//		{
//			bool testFlag = true;

//			_builder.AddNext(
//				_context,
//				(c, o) => NextCommand.Continues.AsAwaitable(),
//				s => testFlag = false);

//			Assert.True(testFlag);
			
//			await _builder
//				.Build(_context)
//				.RunAsync();

//			Assert.False(testFlag);
//		}

//		[Fact]
//		public async Task OnStart_NotSet_NotInvoked()
//		{
//			bool testFlag = true;

//			_builder.AddNext(
//				_context,
//				(c, o) => NextCommand.Continues.AsAwaitable());

//			Assert.True(testFlag);

//			await _builder
//				.Build(_context)
//				.RunAsync();

//			Assert.True(testFlag);
//		}
//	}
//}
