using System;
using System.Linq.Expressions;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query
{
    internal class WithMinimalEdges : IWithMinimalEdges
    {
        public WithMinimalEdges(MemoryGraph graph, Guid fromVertexId)
        {
            Graph = graph;
            FromVertexId = fromVertexId;
        }
        
        private MemoryGraph Graph { get; }
        
        private Guid FromVertexId { get; }
        
        private Func<IReadOnlyEdge, bool>? Filter { get; set; }

        public IToVertex<int> ToVertex(Guid vertexId)
        {
            return new ToVertex<int>(
                Graph,
                FromVertexId,
                vertexId,
                _ => 1,
                0,
                (a, b) => a + b,
                Filter,
                ((_, _) => 0)
            );
        }

        public IWithMinimalEdges Where(Expression<Func<IReadOnlyEdge, bool>> filter)
        {
            Filter = filter.Compile();
            return this;
        }
    }
}