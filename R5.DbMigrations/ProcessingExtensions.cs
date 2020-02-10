using R5.DbMigrations.Engine.Processing;

namespace R5.DbMigrations
{
	public static class StageExtensions
	{
		public static Stage<TPC> Then<TPC>(this Stage<TPC> current, Stage<TPC> next)
			where TPC : PipelineContext
		{
			return current.SetNext(next);
		}
	}
}
