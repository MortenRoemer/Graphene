using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryQueryBuilderEdge : IQueryBuilderEdge
    {
        public MemoryQueryBuilderEdge(MemoryQueryBuilderRoot root) : this(root, null, EdgeSearchMode.All) {}

        public MemoryQueryBuilderEdge(MemoryQueryBuilderRoot root, IEnumerable<Guid> range) : this(root, range, EdgeSearchMode.All) {}

        public MemoryQueryBuilderEdge(MemoryQueryBuilderRoot root, EdgeSearchMode searchMode) : this(root, null, searchMode) {}

        private MemoryQueryBuilderEdge(MemoryQueryBuilderRoot root, IEnumerable<Guid> range, EdgeSearchMode searchMode)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Range = range;
            SearchMode = searchMode;
        }

        private IEnumerable<Guid> Range { get; }

        private MemoryQueryBuilderRoot Root { get; }

        private EdgeSearchMode SearchMode { get; }

        public IQueryBuilderVertex AnyVertices()
        {
            return Root.AddToken(new MemoryQueryBuilderVertex(Root));
        }

        public IGraph Resolve()
        {
            throw new NotImplementedException();
        }

        public IQueryBuilderVertex SourceVertex()
        {
            return Root.AddToken(new MemoryQueryBuilderVertex(Root, VertexSearchMode.Source));
        }

        public IQueryBuilderVertex TargetVertex()
        {
            return Root.AddToken(new MemoryQueryBuilderVertex(Root, VertexSearchMode.Target));
        }

        public IQueryBuilderVertex Vertex(Guid id)
        {
            return Root.AddToken(new MemoryQueryBuilderVertex(Root, new[] { id }));
        }

        public IQueryBuilderVertex Vertices(IEnumerable<Guid> ids)
        {
            return Root.AddToken(new MemoryQueryBuilderVertex(Root, ids));
        }

        public IFilterRoot<IQueryBuilderEdge> Where()
        {
            throw new NotImplementedException();
        }
    }
}