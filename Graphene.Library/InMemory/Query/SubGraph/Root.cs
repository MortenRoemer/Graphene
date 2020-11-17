using System;
using System.Collections.Generic;
using Graphene.Query.SubGraph;

namespace Graphene.InMemory.Query.SubGraph
{
    public class Root : IRoot
    {
        internal Root(IReadOnlyGraph graph)
        {
            Graph = graph;
        }
        
        internal IReadOnlyGraph Graph { get; }
        
        public IVertices Vertices()
        {
            return new Vertices(this, null);
        }

        public IVertices Vertices(IEnumerable<int> ids)
        {
            return new Vertices(this, ids);
        }

        public IEdges Edges()
        {
            return new Edges(this, null);
        }

        public IEdges Edges(IEnumerable<int> ids)
        {
            return new Edges(this, ids);
        }
    }
}