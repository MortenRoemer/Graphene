using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryQueryBuilderRoot : IQueryBuilderRoot
    {
        public MemoryQueryBuilderRoot(MemoryGraph graph)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Tokens = new List<object>();
        }

        private MemoryGraph Graph { get; }

        private List<object> Tokens { get; }

        internal T AddToken<T>(T token)
        {
            Tokens.Add(token);
            return token;
        }

        public IQueryBuilderEdge AnyEdges()
        {
            return AddToken(new MemoryQueryBuilderEdge(this));
        }

        public IQueryBuilderVertex AnyVertex()
        {
            return AddToken(new MemoryQueryBuilderVertex(this));
        }

        public IQueryBuilderEdge Edge(Guid id)
        {
            return AddToken(new MemoryQueryBuilderEdge(this, new[] { id }));
        }

        public IQueryBuilderEdge Edges(IEnumerable<Guid> ids)
        {
            return AddToken(new MemoryQueryBuilderEdge(this, ids));
        }

        public IQueryBuilderVertex Vertex(Guid id)
        {
            return AddToken(new MemoryQueryBuilderVertex(this, new[] { id }));
        }

        public IQueryBuilderVertex Vertices(IEnumerable<Guid> ids)
        {
            return AddToken(new MemoryQueryBuilderVertex(this, ids));
        }
    }
}