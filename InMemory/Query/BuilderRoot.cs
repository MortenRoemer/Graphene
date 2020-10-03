using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class BuilderRoot : IQueryBuilderRoot
    {
        internal BuilderRoot(MemoryGraph graph)
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
            return AddToken(new BuilderEdge(this));
        }

        public IQueryBuilderVertex AnyVertex()
        {
            return AddToken(new BuilderVertex(this));
        }

        public IQueryBuilderEdge Edge(Guid id)
        {
            return AddToken(new BuilderEdge(this, new[] { id }));
        }

        public IQueryBuilderEdge Edges(IEnumerable<Guid> ids)
        {
            return AddToken(new BuilderEdge(this, ids));
        }

        internal IQueryResult Resolve()
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderVertex Vertex(Guid id)
        {
            return AddToken(new BuilderVertex(this, new[] { id }));
        }

        public IQueryBuilderVertex Vertices(IEnumerable<Guid> ids)
        {
            return AddToken(new BuilderVertex(this, ids));
        }
    }
}