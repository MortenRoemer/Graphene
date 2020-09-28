using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryQueryBuilderVertex : IQueryBuilderVertex
    {
        public MemoryQueryBuilderVertex(MemoryQueryBuilderRoot root) : this(root, null, VertexSearchMode.All) {}

        public MemoryQueryBuilderVertex(MemoryQueryBuilderRoot root, IEnumerable<Guid> range) : this(root, range, VertexSearchMode.All) {}

        public MemoryQueryBuilderVertex(MemoryQueryBuilderRoot root, VertexSearchMode searchMode) : this(root, null, searchMode) {}

        public MemoryQueryBuilderVertex(MemoryQueryBuilderRoot root, IEnumerable<Guid> range, VertexSearchMode searchMode)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Range = range;
            SearchMode = searchMode;
        }

        private IEnumerable<Guid> Range { get; }

        private MemoryQueryBuilderRoot Root { get; }

        private VertexSearchMode SearchMode { get; }

        public IQueryBuilderEdge AnyEdges()
        {
            return Root.AddToken(new MemoryQueryBuilderEdge(Root));
        }

        public IQueryBuilderEdge AnyIngoingEdges()
        {
            return Root.AddToken(new MemoryQueryBuilderEdge(Root, EdgeSearchMode.Ingoing));
        }

        public IQueryBuilderEdge AnyOutgoingEdges()
        {
            return Root.AddToken(new MemoryQueryBuilderEdge(Root, EdgeSearchMode.Outgoing));
        }

        public IQueryBuilderEdge Edge(Guid id)
        {
            return Root.AddToken(new MemoryQueryBuilderEdge(Root, new[] { id }));
        }

        public IQueryBuilderEdge Edges(IEnumerable<Guid> ids)
        {
            return Root.AddToken(new MemoryQueryBuilderEdge(Root, ids));
        }

        public IGraph Resolve()
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderRoute RouteToAnyEdge()
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderRoute RouteToAnyVertex()
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderRoute RouteToEdge(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderRoute RouteToEdges(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderRoute RouteToVertex(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderRoute RouteToVertices(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public IFilterRoot<IQueryBuilderVertex> Where()
        {
            throw new NotImplementedException();
        }
    }
}