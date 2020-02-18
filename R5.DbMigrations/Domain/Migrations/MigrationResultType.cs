using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Domain.Migrations
{
	public enum MigrationResultType
	{
		Error,
		Completed,
		//SkippedFromInit
	}
}
