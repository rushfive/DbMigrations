using System;
using System.Collections.Generic;
using System.Text;

namespace R5.DbMigrations.Tests.Engine.Processing
{
	public class ProcessTestException : Exception
	{
		public int Integer { get; }
		public ProcessTestException(int integer) : base()
		{
			Integer = integer;
		}
	}
}
