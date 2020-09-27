using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryQueryBuilderEdge : IQueryBuilderEdge
    {
        public MemoryQueryBuilderEdge(MemoryQueryBuilderRoot root, IEnumerable<Guid> range)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Range = range;
        }

        private IEnumerable<Guid> Range { get; }

        private MemoryQueryBuilderRoot Root { get; }

        public IQueryBuilderVertex AnyVertices()
        {
            throw new NotImplementedException();
        }

        public IGraph Resolve()
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderVertex SourceVertex()
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderVertex TargetVertex()
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderVertex Vertex(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderVertex Vertices(IEnumerable<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public IFilterRoot<IQueryBuilderEdge> Where()
        {
            throw new NotImplementedException();
        }
    }
}