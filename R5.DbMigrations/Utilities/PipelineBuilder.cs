using R5.DbMigrations.Engine.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5.DbMigrations.Utilities
{
	public class PipelineBuilder<TPipeline, TPC>
		where TPipeline : Pipeline<TPC>
		where TPC : PipelineContext
	{
		private readonly List<Stage<TPC>> _stages
			= new List<Stage<TPC>>();

		public PipelineBuilder()
		{
		}

		public static PipelineBuilder<TPipeline, TPC> StartsWith(
			Stage<TPC> stage)
		{
			return new PipelineBuilder<TPipeline, TPC>()
				.AddNext(stage);
		}

		public PipelineBuilder<TPipeline, TPC> AddNext(
			Stage<TPC> stage)
		{
			_stages.Add(stage);
			return this;
		}

		public TPipeline Build(
			TPC context,
			Func<TPC, Task> onStart = null,
			Func<TPC, Task> onEnd = null,
			Func<Exception, TPC, Task> onError = null)
		{
			var curr = _stages.First();
			_stages.Skip(1).ForEach(s =>
			{
				curr = curr.SetNext(s);
			});

			var pipeline = Activator.CreateInstance(
				typeof(TPipeline), 
				_stages.First(), 
				context,
				onStart,
				onEnd,
				onError);

			return pipeline as TPipeline;
		}
	}
}
