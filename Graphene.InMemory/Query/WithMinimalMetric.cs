using System;
using System.Linq.Expressions;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query
{
    internal class WithMinimalMetric<TMetric> : IWithMinimalMetric<TMetric> where TMetric : IComparable<TMetric>
    {
        public WithMinimalMetric(
            MemoryGraph graph,
            Guid fromVertexId,
            Func<IReadOnlyEdge, TMetric> metricFunction,
            TMetric defaultCost,
            Func<TMetric, TMetric, TMetric> accumulatorFunction)
        {
            Graph = graph;
            FromVertexId = fromVertexId;
            MetricFunction = metricFunction;
            DefaultCost = defaultCost;
            AccumulatorFunction = accumulatorFunction;
            HeuristicFunction = (_, _) => DefaultCost;
        }

        private MemoryGraph Graph { get; }
        
        private Guid FromVertexId { get; }
        
        private Func<IReadOnlyEdge, TMetric> MetricFunction { get; }
        
        private TMetric DefaultCost { get; }
        
        private Func<TMetric, TMetric, TMetric> AccumulatorFunction { get; }

        private Func<IReadOnlyEdge, bool>? Filter { get; set; }
        
        private Func<IReadOnlyVertex, IReadOnlyVertex, TMetric> HeuristicFunction { get; set; }

        public IToVertex<TMetric> ToVertex(Guid vertexId)
        {
            return new ToVertex<TMetric>(
                Graph,
                FromVertexId,
                vertexId,
                MetricFunction,
                DefaultCost,
                AccumulatorFunction,
                Filter,
                HeuristicFunction
            );
        }

        public IWithMinimalMetric<TMetric> Where(Expression<Func<IReadOnlyEdge, bool>> filter)
        {
            Filter = filter.Compile();
            return this;
        }

        public IWithMinimalMetric<TMetric> WithHeuristic(Expression<Func<IReadOnlyVertex, IReadOnlyVertex, TMetric>> heuristicFunction)
        {
            HeuristicFunction = heuristicFunction.Compile();
            return this;
        }
    }
}