using System;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class QueryRoot : IQueryRoot
    {
        internal QueryRoot(IReadOnlyGraph graph)
        {
            Graph = graph;
        }
        
        private IReadOnlyGraph Graph { get; }
        
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