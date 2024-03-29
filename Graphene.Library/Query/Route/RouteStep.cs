namespace Graphene.Query.Route
{
    public readonly struct RouteStep
    {
        public RouteStep(IReadOnlyEdge edge, IReadOnlyVertex vertex)
        {
            Edge = edge;
            Vertex = vertex;
        }
        
        public IReadOnlyEdge Edge { get; }
        
        public IReadOnlyVertex Vertex { get; }
    }
}