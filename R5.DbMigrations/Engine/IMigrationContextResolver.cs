using R5.DbMigrations.Domain.Versioning;
using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Engine
{
	public interface IMigrationContextResolver<T>
	{
		T Get();
	}
}
