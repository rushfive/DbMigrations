using R5.DbMigrations.Engine.Processing;

namespace R5.DbMigrations
{
	public static class StageExtensions
	{
		public static Stage<TPC, TMC> Then<TPC, TMC>(this Stage<TPC, TMC> current, Stage<TPC, TMC> next)
			where TPC : PipelineContext<TMC>
		{
			return current.SetNext(next);
		}
	}
}
