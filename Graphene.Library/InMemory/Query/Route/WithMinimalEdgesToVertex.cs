using System;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class WithMinimalEdgesToVertex : IToVertex<int>
    {
        private static readonly Lazy<RouteResolver<int>> _routeResolver = new(false);
        
        internal WithMinimalEdgesToVertex(WithMinimalEdges withMinimalEdges, int vertexId)
        {
            WithMinimalEdges = withMinimalEdges;
            VertexId = vertexId;
        }
        
        private WithMinimalEdges WithMinimalEdges { get; }
        
        private int VertexId { get; }
        
        private CachedResult<RouteResult<int>>? CachedResult { get; set; }
        
        public RouteResult<int> Resolve()
        {
            var graph = WithMinimalEdges.FromVertex.Root.Graph;
            var currentTimestamp = graph.DataVersion;

            if (CachedResult.HasValue && CachedResult.Value.TimeStamp == currentTimestamp)
                return CachedResult.Value.Result;
            
            var origin = graph.Vertices.Get(WithMinimalEdges.FromVertex.VertexId);
            var target = graph.Vertices.Get(VertexId);

            var result = _routeResolver.Value.SolveForMinimalMetric(
                origin,
                target,
                WithMinimalEdges.Filter,
                _ => 1,
                (_, _) => 0,
                ((first,second) => first + second)
            );

            CachedResult = new CachedResult<RouteResult<int>>(currentTimestamp, result);
            return result;
        }
    }
}