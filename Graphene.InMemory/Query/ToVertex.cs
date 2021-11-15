using System;
using System.Threading.Tasks;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query
{
    public class ToVertex<TMetric> : IToVertex<TMetric> where TMetric : IComparable<TMetric>
    {
        public ToVertex(
            MemoryGraph graph,
            Guid fromVertexId,
            Guid toVertexId,
            Func<IReadOnlyEdge, TMetric> metricFunction,
            TMetric defaultCost,
            Func<TMetric, TMetric, TMetric> accumulatorFunction,
            Func<IReadOnlyEdge, bool>? filter,
            Func<IReadOnlyVertex, IReadOnlyVertex, TMetric> heuristicFunction)
        {
            Graph = graph;
            FromVertexId = fromVertexId;
            ToVertexId = toVertexId;
            MetricFunction = metricFunction;
            DefaultCost = defaultCost;
            AccumulatorFunction = accumulatorFunction;
            Filter = filter;
            HeuristicFunction = heuristicFunction;
        }

        private MemoryGraph Graph { get; }
        
        private Guid FromVertexId { get; }
        
        private Guid ToVertexId { get; }
        
        private Func<IReadOnlyEdge, TMetric> MetricFunction { get; }
        
        private Func<TMetric, TMetric, TMetric> AccumulatorFunction { get; }

        private TMetric DefaultCost { get; }

        private Func<IReadOnlyEdge, bool>? Filter { get; }
        
        private Func<IReadOnlyVertex, IReadOnlyVertex, TMetric> HeuristicFunction { get; }
        
        public async Task<RouteResult<TMetric>> Resolve()
        {
            return await Graph.FindShortestRoute(
                Graph,
                FromVertexId,
                ToVertexId,
                MetricFunction,
                AccumulatorFunction,
                DefaultCost,
                Filter,
                HeuristicFunction);
        }
    }
}