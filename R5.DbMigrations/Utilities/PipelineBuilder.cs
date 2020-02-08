using R5.DbMigrations.Engine.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace R5.DbMigrations.Utilities
{
	public class PipelineBuilder<TPipeline, TPC, TSC>
		where TPipeline : Pipeline<TPC, TSC>
		where TPC : PipelineContext<TSC>
	{
		private readonly List<Stage<TPC, TSC>> _stages
			= new List<Stage<TPC, TSC>>();

		private PipelineBuilder(
			Stage<TPC, TSC> head)
		{
			_stages.Add(head);
		}

		public static PipelineBuilder<TPipeline, TPC, TSC> StartsWith(
			Stage<TPC, TSC> stage)
		{
			return new PipelineBuilder<TPipeline, TPC, TSC>(stage);
		}

		public PipelineBuilder<TPipeline, TPC, TSC> AddNext(
			Stage<TPC, TSC> stage)
		{
			_stages.Add(stage);
			return this;
		}

		public TPipeline Build(TPC context)
		{
			var curr = _stages.First();
			_stages.Skip(1).ForEach(s =>
			{
				curr = curr.SetNext(s);
			});

			var pipeline = Activator.CreateInstance(typeof(TPipeline), _stages.First(), context);
			return pipeline as TPipeline;
		}
	}
}
