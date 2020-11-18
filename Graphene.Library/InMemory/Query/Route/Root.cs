using System;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class Root : IRoot
    {
        internal Root(IReadOnlyGraph graph)
        {
            Graph = graph;
        }
        
        internal IReadOnlyGraph Graph { get; }
        
        public IFromVertex FromVertex(int vertexId)
        {
            if (!Graph.Vertices.Contains(vertexId))
                throw new ArgumentException($"vertex with id {vertexId} does not exist");
            
            return new FromVertex(this, vertexId);
        }
    }
}