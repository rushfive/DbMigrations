using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.DevTest
{
	public static class TestPipelineWithAutoSerilog
	{
		public static async Task RunTestAsync()
		{
			var services = new ServiceCollection();
			services.AddLogging();
			//services.AddScoped<MongoPipelineTest>();

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


			//var test = container.Resolve<MongoPipelineTest>();
			//await test.RunTestAsync();
		}
	}
}
