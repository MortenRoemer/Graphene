using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class BuilderVertex : IQueryBuilderVertex
    {
        internal BuilderVertex(BuilderRoot root) : this(root, null, VertexSearchMode.All) {}

        internal BuilderVertex(BuilderRoot root, IEnumerable<long> range) : this(root, range, VertexSearchMode.All) {}

        internal BuilderVertex(BuilderRoot root, VertexSearchMode searchMode) : this(root, null, searchMode) {}

        internal BuilderVertex(BuilderRoot root, IEnumerable<long> range, VertexSearchMode searchMode)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Range = range;
            SearchMode = searchMode;
        }

        private IEnumerable<long> Range { get; }

        private BuilderRoot Root { get; }

        private VertexSearchMode SearchMode { get; }

        private FilterRoot<IQueryBuilderVertex> Filter { get; set; }

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

        public IQueryBuilderEdge Edge(long id)
        {
            return Root.AddToken(new BuilderEdge(Root, new[] { id }));
        }

        public IQueryBuilderEdge Edges(IEnumerable<long> ids)
        {
            return Root.AddToken(new BuilderEdge(Root, ids));
        }

        public bool Resolve(out IQueryResult result)
        {
            return Root.Resolve(out result);
        }

        public IQueryBuilderRoute RouteToAnyEdge()
        {
            return Root.AddToken(new BuilderRoute(Root, RouteSearchMode.Edge));
        }

        public IQueryBuilderRoute RouteToAnyVertex()
        {
            return Root.AddToken(new BuilderRoute(Root, RouteSearchMode.Vertex));
        }

        public IQueryBuilderRoute RouteToEdge(long id)
        {
            return Root.AddToken(new BuilderRoute(Root, RouteSearchMode.Edge, new[] { id }));
        }

        public IQueryBuilderRoute RouteToEdges(IEnumerable<long> ids)
        {
            return Root.AddToken(new BuilderRoute(Root, RouteSearchMode.Edge, ids));
        }

        public IQueryBuilderRoute RouteToVertex(long id)
        {
            return Root.AddToken(new BuilderRoute(Root, RouteSearchMode.Vertex, new[] { id }));
        }

        public IQueryBuilderRoute RouteToVertices(IEnumerable<long> ids)
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