using System;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class WithMinimalMetricToVertex<TMetric> : IToVertex<TMetric> where TMetric : IComparable<TMetric>
    {
        internal WithMinimalMetricToVertex(WithMinimalMetric<TMetric> withMinimalMetric, int targetId)
        {
            WithMinimalMetric = withMinimalMetric;
            TargetId = targetId;
        }
        
        private WithMinimalMetric<TMetric> WithMinimalMetric { get; }
        
        private int TargetId { get; }
        
        public RouteResult<TMetric> Resolve()
        {
            var graph = WithMinimalMetric.FromVertex.Root.Graph;
            var origin = graph.Vertices.Get(WithMinimalMetric.FromVertex.VertexId);
            var target = graph.Vertices.Get(TargetId);

            return RouteResolver.SolveForMinimalMetric(
                origin,
                target,
                WithMinimalMetric.Filter,
                WithMinimalMetric.MetricFunction,
                WithMinimalMetric.Heuristic ?? ((from, to) => default),
                WithMinimalMetric.Accumulator
            );
        }
    }
}