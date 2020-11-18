using System;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class WithMinimalEdges : IWithMinimalEdges
    {
        internal WithMinimalEdges(FromVertex fromVertex)
        {
            FromVertex = fromVertex;
        }
        
        internal FromVertex FromVertex { get; }
        
        internal Func<IReadOnlyEdge, bool> Filter { get; private set; }
        
        public IToVertex<int> ToVertex(int vertexId)
        {
            if (!FromVertex.Root.Graph.Vertices.Contains(vertexId))
                throw new ArgumentException($"vertex with id {vertexId} does not exist");
            
            return new WithMinimalEdgesToVertex(this, vertexId);
        }

        public IWithMinimalEdges Where(Func<IReadOnlyEdge, bool> filter)
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));
            
            Filter = Filter is null ? filter : edge => Filter(edge) && filter(edge);
            return this;
        }
    }
}