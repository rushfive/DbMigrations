using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Engine.Processing
{
	public abstract class PipelineContext<TMigrationContext>
	{
		// pipeline PER migration
		public DbVersion MigrationVersion { get; set; }
		public IMigrationContextResolver<TMigrationContext> MigrationContextResolver { get; set; }
	}
}
