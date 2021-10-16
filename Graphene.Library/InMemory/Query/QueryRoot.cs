using System;
using System.Collections.Generic;
using Graphene.InMemory.Query.Entity;
using Graphene.Query;
using Graphene.Query.Entity;

namespace Graphene.InMemory.Query
{
    public class QueryRoot : IQueryRoot
    {
        internal QueryRoot(IReadOnlyGraph graph)
        {
            Graph = graph;
        }
        
        private IReadOnlyGraph Graph { get; }

        public IVertices Vertices()
        {
            return new Vertices(Graph, null);
        }

        public IVertices Vertices(IEnumerable<int>? range)
        {
            if (range is null)
                throw new ArgumentNullException(nameof(range));
            
            return new Vertices(Graph, range);
        }

        public IEdges Edges()
        {
            return new Edges(Graph, null);
        }

        public IEdges Edges(IEnumerable<int>? range)
        {
            if (range is null)
                throw new ArgumentNullException(nameof(range));
            
            return new Edges(Graph, range);
        }

        public Graphene.Query.Route.IRoot Route()
        {
            return new Route.Root(Graph);
        }

        public Graphene.Query.SubGraph.IRoot SubGraph()
        {
            return new SubGraph.Root(Graph);
        }
    }
}