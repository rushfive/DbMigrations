namespace R5.DbMigrations.Engine.Processing
{
	public abstract class NextCommand
	{
		public static readonly Continue Continues = new Continue();
		public static ContinueWith ContinuesWith(object result) => new ContinueWith(result);
		public static readonly End Ends = new End();

		public class Continue : NextCommand { }

		public class ContinueWith : NextCommand
		{
			public readonly object Result;
			public ContinueWith(object result) => Result = result;
		}

		public class End : NextCommand { }
	}
}
