using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using R5.DbMigrations.Domain.Versioning;
using R5.DbMigrations.Engine;
using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using R5.DbMigrations.Mongo.Processing;
using R5.DbMigrations.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace R5.DbMigrations.DevTest
{
	
	class Program
	{
		static async Task Main(string[] args)
		{
			List<MongoMigration> mongoMigrations = ExistingMigrationsFinder
				.GetMigrationsDerivedFrom<MongoMigration>(Assembly.GetExecutingAssembly())
				.ToList();

			//var executingAssembly = Assembly.GetExecutingAssembly();
			//var callingAssembly = Assembly.GetCallingAssembly();
			//var entryAssembly = Assembly.GetEntryAssembly();

			Console.WriteLine("Hello World!");
		}
	}
}
    