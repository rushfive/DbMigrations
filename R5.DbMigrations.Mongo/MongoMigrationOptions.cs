using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Mongo
{
	public class MongoMigrationOptions
	{
		public bool UseTransaction { get; set; }
		public bool RetryWithoutTransactionOnFail { get; set; }
	}
}
