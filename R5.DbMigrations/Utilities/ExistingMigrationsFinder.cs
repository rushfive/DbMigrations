using R5.DbMigrations.Domain.Migrations;
using R5.DbMigrations.Engine.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace R5.DbMigrations.Utilities
{
	public static class ExistingMigrationsFinder
	{
		// Make sure to pass in the correct assembly
		public static IEnumerable<TBaseMigration> GetMigrationsDerivedFrom<TBaseMigration, TContext>(Assembly searchAssembly)
			where TBaseMigration : DbMigration<TContext>, IComparable<TBaseMigration>
			where TContext : MigrationContext
				=> searchAssembly
					.GetTypes()
					.Where(t => t.IsDirectDerivationOf<TBaseMigration>())
					.Select(t => (TBaseMigration)Activator.CreateInstance(t))
					.OrderBy(t => t);
	}
}
