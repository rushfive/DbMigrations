using R5.DbMigrations.Engine.Processing;
using R5.DbMigrations.Mongo.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Mongo.Processing
{
	public class MongoMigrationPipeline : Pipeline<MongoMigrationContext>
	{
		private readonly MongoMigrationOptions _options;

		public MongoMigrationPipeline(
			MongoMigrationStage headStage, 
			MongoMigrationContext context,
			MongoMigrationOptions options)
			: base(headStage, context)
		{
			_options = options;
		}

		protected override Func<Task> OnStart => () =>
		{
			_context.Database.StartTransaction();
			return Task.CompletedTask;
		};

		protected override Func<Task> OnEnd => 
			() => _context.Database.CommitTransactionAsync();

		protected override Func<Exception, Task> OnError => 
			ex => _context.Database.AbortTransactionAsync();
	}
}
