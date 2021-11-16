using System;
using Graphene.Query;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query
{
    internal class QueryRoot : IQueryRoot, IRoot
    {
        public QueryRoot(MemoryGraph graph)
        {
            Graph = graph;
        }
        
        private MemoryGraph Graph { get; }
        
        public IRoot Route()
        {
            return this;
        }

        public IFromVertex FromVertex(Guid vertexId)
        {
            return new FromVertex(Graph, vertexId);
        }
    }
}