using System;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class WithMinimalMetric<TMetric> : IWithMinimalMetric<TMetric> where TMetric: IComparable<TMetric>
    {
        internal WithMinimalMetric(
            FromVertex fromVertex, 
            Func<IReadOnlyEdge, TMetric> metricFunction,
            Func<TMetric, TMetric, TMetric> accumulatorFunction
        )
        {
            FromVertex = fromVertex;
            MetricFunction = metricFunction;
            Accumulator = accumulatorFunction;
        }
        
        internal FromVertex FromVertex { get; }
        
        internal Func<IReadOnlyEdge, TMetric> MetricFunction { get; }
        
        internal Func<TMetric, TMetric, TMetric> Accumulator { get; }
        
        internal Func<IReadOnlyEdge, bool> Filter { get; private set; }
        
        internal Func<IReadOnlyVertex, IReadOnlyVertex, TMetric> Heuristic { get; private set; }
        
        public IToVertex<TMetric> ToVertex(int targetId)
        {
            if (!FromVertex.Root.Graph.Vertices.Contains(targetId))
                throw new ArgumentException($"vertex with id {targetId} does not exist");
            
            return new WithMinimalMetricToVertex<TMetric>(this, targetId);
        }

        public IWithMinimalMetric<TMetric> Where(Func<IReadOnlyEdge, bool> filter)
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));
            
            Filter = Filter is null ? filter : edge => Filter(edge) && filter(edge);
            return this;
        }

        public IWithMinimalMetric<TMetric> WithHeuristic(Func<IReadOnlyVertex, IReadOnlyVertex, TMetric> heuristicFunction)
        {
            Heuristic = heuristicFunction ?? throw new ArgumentNullException(nameof(heuristicFunction));
            return this;
        }
    }
}