using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class BuilderEdge : IQueryBuilderEdge
    {
        internal BuilderEdge(BuilderRoot root) : this(root, null, EdgeSearchMode.All) {}

        internal BuilderEdge(BuilderRoot root, IEnumerable<Guid> range) : this(root, range, EdgeSearchMode.All) {}

        internal BuilderEdge(BuilderRoot root, EdgeSearchMode searchMode) : this(root, null, searchMode) {}

        internal BuilderEdge(BuilderRoot root, IEnumerable<Guid> range, EdgeSearchMode searchMode)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Range = range;
            SearchMode = searchMode;
        }

        private IEnumerable<Guid> Range { get; }

        private BuilderRoot Root { get; }

        private EdgeSearchMode SearchMode { get; }

        private FilterRoot<IQueryBuilderEdge> Filter { get; set; }

        public IQueryBuilderVertex AnyVertices()
        {
            return Root.AddToken(new BuilderVertex(Root));
        }

        public IQueryResult Resolve()
        {
            return Root.Resolve();
        }

        public IQueryBuilderVertex SourceVertex()
        {
            return Root.AddToken(new BuilderVertex(Root, VertexSearchMode.Source));
        }

        public IQueryBuilderVertex TargetVertex()
        {
            return Root.AddToken(new BuilderVertex(Root, VertexSearchMode.Target));
        }

        public IQueryBuilderVertex Vertex(Guid id)
        {
            return Root.AddToken(new BuilderVertex(Root, new[] { id }));
        }

        public IQueryBuilderVertex Vertices(IEnumerable<Guid> ids)
        {
            return Root.AddToken(new BuilderVertex(Root, ids));
        }

        public IFilterRoot<IQueryBuilderEdge> Where()
        {
            Filter = new FilterRoot<IQueryBuilderEdge>(this);
            return Filter;
        }
    }
}