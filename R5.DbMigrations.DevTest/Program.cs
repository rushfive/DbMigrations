using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R5.DbMigrations.DevTest.MongoPipeline;
using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using R5.DbMigrations.Mongo.Processing;
using R5.DbMigrations.Utilities;
using Serilog;
using System;
using System.Threading.Tasks;

namespace R5.DbMigrations.DevTest
{
	public class BuilderTestPipeline : Pipeline<TestBuilderStage.PC, TestBuilderStage.SC>
	{
		public BuilderTestPipeline(
			Stage<TestBuilderStage.PC, TestBuilderStage.SC> headStage,
			TestBuilderStage.PC context)
			: base(headStage, context)
		{

		}
	}
	public class TestBuilderStage : Stage<TestBuilderStage.PC, TestBuilderStage.SC>
	{
		public TestBuilderStage()
			: base(new PC(default, default))
		{

		}

		protected override Task<NextCommand> ProcessAsync(SC context, object input)
		{
			throw new NotImplementedException();
		}

		public class PC : PipelineContext<SC>
		{
			public PC(DbVersion version, IStageContextResolver<SC> stageContextResolver) : base(version, stageContextResolver)
			{
			}
		}
		public class SC
		{

		}
	}
	class Program
	{
		static async Task Main(string[] args)
		{
			var ctx = new TestBuilderStage.PC(default, default);
			var s = new TestBuilderStage();

			var pb = PipelineBuilder<BuilderTestPipeline, TestBuilderStage.PC, TestBuilderStage.SC>.StartsWith(s);

			var pppppppp = pb.Build(ctx);


			//////////

			var services = new ServiceCollection();
			services.AddLogging();
			services.AddScoped<MongoPipelineTest>();

			// Initialize Autofac
			var builder = new ContainerBuilder();
			// Use the Populate method to register services which were registered
			// to IServiceCollection
			builder.Populate(services);

			// Build the final container
			IContainer container = builder.Build();

			var loggerFactory = container.Resolve<ILoggerFactory>();
			loggerFactory//.AddConsole()
				.AddSerilog();

			var logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				//.WriteTo.File(new RenderedCompactJsonFormatter(), "TestLogs.txt")
				.CreateLogger();
			Log.Logger = logger;


			var test = container.Resolve<MongoPipelineTest>();
			await test.RunTestAsync();

			//await MongoPipelineTest.RunTestAsync();




			Console.WriteLine("Hello World!");
		}
	}
}
    