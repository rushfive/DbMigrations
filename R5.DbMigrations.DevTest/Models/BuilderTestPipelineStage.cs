//using R5.DbMigrations.Domain.Versioning;
//using R5.DbMigrations.Engine;
//using R5.DbMigrations.Engine.Processing;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace R5.DbMigrations.DevTest.Models
//{
//	public class BuilderTestPipeline : Pipeline<TestBuilderStage.PC>
//	{
//		public BuilderTestPipeline(
//			Stage<TestBuilderStage.PC, TestBuilderStage.SC> headStage,
//			TestBuilderStage.PC context)
//			: base(headStage, context)
//		{

//		}
//	}
//	public class TestBuilderStage : Stage<TestBuilderStage.PC, TestBuilderStage.SC>
//	{
//		public TestBuilderStage()
//			: base(new PC(default, default))
//		{

//		}

//		protected override Task<NextCommand> ProcessAsync(SC context, object input)
//		{
//			throw new NotImplementedException();
//		}

//		public class PC : PipelineContext<SC>
//		{
//			public PC(DbVersion version, IMigrationContextResolver<SC> stageContextResolver) : base(version, stageContextResolver)
//			{
//			}
//		}
//		public class SC
//		{

//		}
//	}
//}
