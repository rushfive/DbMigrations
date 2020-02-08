using R5.DbMigrations.DevTest.MongoPipeline;
using System;
using System.Threading.Tasks;

namespace R5.DbMigrations.DevTest
{
	class Program
	{
		static async Task Main(string[] args)
		{
			await MongoPipelineTest.RunTestAsync();




			Console.WriteLine("Hello World!");
		}
	}
}
    