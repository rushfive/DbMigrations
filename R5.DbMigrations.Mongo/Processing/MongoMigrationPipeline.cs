using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo.Processing
{
	public class MongoMigrationPipeline : Pipeline<MongoMigrationContext>
	{
		//private readonly MongoMigrationOptions _options;

		public MongoMigrationPipeline(
			List<MongoMigrationStage> stages,
			MongoMigrationContext context)
			: base(stages.Cast<Stage<MongoMigrationContext>>().ToList(), context)
		{
			//_options = options;
		}

		//protected override Func<Task> OnStart => () =>
		//{
		//	//_context.DbContext.StartTransaction();
		//	return Task.CompletedTask;
		//};

		//protected override Func<Task> OnEnd =>
		//	() => _context.DbContext.CommitTransactionAsync();

		//protected override Func<Exception, Task> OnError =>
		//	ex => _context.DbContext.AbortTransactionAsync();
	}
}
