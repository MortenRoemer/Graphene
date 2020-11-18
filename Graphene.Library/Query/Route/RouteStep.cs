using System;

namespace Graphene.Query.Route
{
    public readonly struct RouteStep
    {
        public RouteStep(IReadOnlyEdge edge, IReadOnlyVertex vertex)
        {
            Edge = edge ?? throw new ArgumentNullException(nameof(edge));
            Vertex = vertex ?? throw new ArgumentNullException(nameof(vertex));
        }
        
        public IReadOnlyEdge Edge { get; }
        
        public IReadOnlyVertex Vertex { get; }
    }
}