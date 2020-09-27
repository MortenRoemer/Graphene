using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryQueryBuilderVertex : IQueryBuilderVertex
    {
        public MemoryQueryBuilderVertex(MemoryQueryBuilderRoot root, IEnumerable<Guid> range)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Range = range;
        }

        private IEnumerable<Guid> Range { get; }

        private MemoryQueryBuilderRoot Root { get; }

        public IQueryBuilderEdge AnyEdges()
        {
            return Root.AddToken(new MemoryQueryBuilderEdge(Root, null));
        }

        public IQueryBuilderEdge AnyIngoingEdges()
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderEdge AnyOutgoingEdges()
        {
            throw new NotImplementedException();
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