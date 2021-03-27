using System;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class WithMinimalMetricToVertex<TMetric> : IToVertex<TMetric> where TMetric : IComparable<TMetric>
    {
        private readonly RouteResolver<TMetric> _routeResolver;
        
        internal WithMinimalMetricToVertex(WithMinimalMetric<TMetric> withMinimalMetric, int targetId)
        {
            WithMinimalMetric = withMinimalMetric;
            TargetId = targetId;
            _routeResolver = new RouteResolver<TMetric>();
        }
        
        private WithMinimalMetric<TMetric> WithMinimalMetric { get; }
        
        private int TargetId { get; }
        
        private CachedResult<RouteResult<TMetric>>? CachedResult { get; set; }
        
        public RouteResult<TMetric> Resolve()
        {
            var graph = WithMinimalMetric.FromVertex.Root.Graph;
            var currentTimeStamp = graph.DataVersion;

            if (CachedResult.HasValue && CachedResult.Value.TimeStamp == currentTimeStamp)
                return CachedResult.Value.Result;
            
            var origin = graph.Vertices.Get(WithMinimalMetric.FromVertex.VertexId);
            var target = graph.Vertices.Get(TargetId);

            var result = _routeResolver.SolveForMinimalMetric(
                origin,
                target,
                WithMinimalMetric.Filter,
                WithMinimalMetric.MetricFunction,
                WithMinimalMetric.Heuristic ?? ((from, to) => default),
                WithMinimalMetric.Accumulator
            );

            CachedResult = new CachedResult<RouteResult<TMetric>>(currentTimeStamp, result);
            return result;
        }
    }
}