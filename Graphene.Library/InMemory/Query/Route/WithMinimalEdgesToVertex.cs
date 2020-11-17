using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class WithMinimalEdgesToVertex : IToVertex<int>
    {
        internal WithMinimalEdgesToVertex(WithMinimalEdges withMinimalEdges, int vertexId)
        {
            WithMinimalEdges = withMinimalEdges;
            VertexId = vertexId;
        }
        
        private WithMinimalEdges WithMinimalEdges { get; }
        
        private int VertexId { get; }
        
        public RouteResult<int> Resolve()
        {
            var graph = WithMinimalEdges.FromVertex.Root.Graph;
            var origin = graph.Vertices.Get(WithMinimalEdges.FromVertex.VertexId);
            var target = graph.Vertices.Get(VertexId);

            return DjikstraRouteResolver.SolveForMinimalMetric(
                origin,
                target,
                WithMinimalEdges.Filter,
                edge => 1,
                ((first,second) => first + second)
            );
        }
    }
}