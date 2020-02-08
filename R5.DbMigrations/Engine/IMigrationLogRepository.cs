using R5.DbMigrations.Domain.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Engine
{
	public interface IMigrationLogRepository<TMigration>
		where TMigration : DbMigration
	{
		Task SaveAppliedAttemptAsync(
			MigrationLog.ApplyAttempt attempt,
			TMigration migration);
	}
}
