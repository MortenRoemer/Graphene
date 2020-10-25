using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class BuilderVertex : IQueryBuilderVertex
    {
        internal BuilderVertex(BuilderRoot root) : this(root, null, VertexSearchMode.All) {}

        internal BuilderVertex(BuilderRoot root, IEnumerable<ulong> range) : this(root, range, VertexSearchMode.All) {}

        internal BuilderVertex(BuilderRoot root, VertexSearchMode searchMode) : this(root, null, searchMode) {}

        internal BuilderVertex(BuilderRoot root, IEnumerable<ulong> range, VertexSearchMode searchMode)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Range = range;
            SearchMode = searchMode;
        }

        internal IEnumerable<ulong> Range { get; }

        private BuilderRoot Root { get; }

        internal VertexSearchMode SearchMode { get; }

        internal FilterRoot<IQueryBuilderVertex> Filter { get; set; }

        public IQueryBuilderEdge AnyEdges()
        {
            return Root.AddToken(new BuilderEdge(Root));
        }

        public IQueryBuilderEdge AnyIngoingEdges()
        {
            return Root.AddToken(new BuilderEdge(Root, EdgeSearchMode.Ingoing));
        }

        public IQueryBuilderEdge AnyOutgoingEdges()
        {
            return Root.AddToken(new BuilderEdge(Root, EdgeSearchMode.Outgoing));
        }

        public IQueryBuilderEdge Edge(ulong id)
        {
            return Root.AddToken(new BuilderEdge(Root, new[] { id }));
        }

        public IQueryBuilderEdge Edges(IEnumerable<ulong> ids)
        {
            return Root.AddToken(new BuilderEdge(Root, ids));
        }

        public bool Resolve(out IQueryResult result)
        {
            return Root.Resolve(out result);
        }

        public IQueryBuilderRoute RouteToAnyVertex()
        {
            return Root.AddToken(new BuilderRoute(Root, RouteSearchMode.Vertex));
        }

        public IQueryBuilderRoute RouteToVertex(ulong id)
        {
            return Root.AddToken(new BuilderRoute(Root, RouteSearchMode.Vertex, new[] { id }));
        }

        public IQueryBuilderRoute RouteToVertices(IEnumerable<ulong> ids)
        {
            return Root.AddToken(new BuilderRoute(Root, RouteSearchMode.Vertex, ids));
        }

        public IFilterRoot<IQueryBuilderVertex> Where()
        {
            Filter = new FilterRoot<IQueryBuilderVertex>(this);
            return Filter;
        }
    }
}