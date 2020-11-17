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
        
        public IToVertex ToVertex(int vertexId)
        {
            return new ToVertex(this, vertexId);
        }

        public IWithMinimalEdges Where(Func<IReadOnlyEdge, bool> filter)
        {
            Filter = Filter is null ? filter : edge => Filter(edge) && filter(edge);
            return this;
        }
    }
}