using MongoDB.Driver;
using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Database;
using R5.DbMigrations.Mongo.Migrations;
using R5.DbMigrations.Mongo.Processing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.DevTest.MongoPipeline
{
	public static class MongoPipelineTest
	{
		private static readonly MongoMigrationOptions _options = new MongoMigrationOptions
		{
			UseTransaction = true
		};
		private static readonly string _connectionStr = "mongodb://mongo1:9560,mongo2:9561/DbUpgradeEval?replicaSet=dockerdev";

		public static Task RunTestAsync()
		{
			var version = new DbVersion("2021.3.3", "5.5.5");

			IMigrationContextResolver<MongoMigrationContext> contextResolver
				= new MongoMigrationContextResolver(_options, _connectionStr);

			var pipelineContext = new MongoPipelineContext
			{
				MigrationVersion = version,
				MigrationContextResolver = contextResolver
			};

			var headStage = BuildStages(_options, pipelineContext);
			var pipeline = new MongoTestPipeline(headStage, pipelineContext);

			return pipeline.RunAsync();
		}

		private static Stage<MongoPipelineContext, MongoMigrationContext>
			BuildStages(MongoMigrationOptions options, MongoPipelineContext context)
		{
			var stage1 = new TestStage1(context);
			var stage2 = new TestStage2(context);
			var stage3 = new TestStage3(context);

			stage1
				.Then(stage2)
				.Then(stage3);

			return stage1;
		}
	}

	public class MongoTestPipeline : Pipeline<MongoPipelineContext, MongoMigrationContext>
	{
		public MongoTestPipeline(
			Stage<MongoPipelineContext, MongoMigrationContext> headStage,
			MongoPipelineContext context)
			: base(headStage, context)
		{

		}
	}

	public class TestStage1 : Stage<MongoPipelineContext, MongoMigrationContext>
	{
		public TestStage1(MongoPipelineContext context)
			: base(context)
		{
		}

		protected override Task<NextCommand> ProcessAsync(MongoMigrationContext context, object input)
		{
			Console.WriteLine($"Processing stage '{nameof(TestStage1)}' with input: {input}");
			return Task.FromResult<NextCommand>(NextCommand.Continues);
		}
	}

	public class TestStage2 : Stage<MongoPipelineContext, MongoMigrationContext>
	{
		public TestStage2(MongoPipelineContext context)
			: base(context)
		{
		}

		protected override Task<NextCommand> ProcessAsync(MongoMigrationContext context, object input)
		{
			Console.WriteLine($"Processing stage '{nameof(TestStage2)}' with input: {input}");
			var result = "this is a RESULT from stage 2!";
			//throw new Exception("test stage 2 failed!");
			return Task.FromResult<NextCommand>(NextCommand.ContinuesWith(result));
		}
	}

	public class TestStage3 : Stage<MongoPipelineContext, MongoMigrationContext>
	{
		public TestStage3(MongoPipelineContext context)
			: base(context)
		{
		}

		protected override Task<NextCommand> ProcessAsync(MongoMigrationContext context, object input)
		{
			Console.WriteLine($"Processing stage '{nameof(TestStage3)}' with input: {input}");
			return Task.FromResult<NextCommand>(NextCommand.Ends);
		}
	}
}
