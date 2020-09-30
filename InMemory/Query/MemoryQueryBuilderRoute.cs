using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryQueryBuilderRoute : IQueryBuilderRoute
    {
        public MemoryQueryBuilderRoute(MemoryQueryBuilderRoot root, RouteSearchMode mode) : this(root, mode, null) {}

        public MemoryQueryBuilderRoute(MemoryQueryBuilderRoot root, RouteSearchMode mode, IEnumerable<Guid> range)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Mode = mode;
            Range = range;
        }

        private MemoryQueryBuilderRoot Root { get; }

        private RouteSearchMode Mode { get; }

        private IEnumerable<Guid> Range { get; }

        private long? EdgeHopLimit { get; set; }

        private long? VertexHopLimit { get; set; }

        private MemoryFilterRoot<IQueryBuilderRoute> TargetFilter { get; set; }

        public IGraph Resolve()
        {
            return Root.Resolve();
        }

        public IFilterRoot<IQueryBuilderRoute> Where()
        {
            TargetFilter = new MemoryFilterRoot<IQueryBuilderRoute>(this);
            return TargetFilter;
        }

        public IGroupFilterRoot<IQueryBuilderRoute> WhereAnyHopEdge()
        {
            throw new System.NotImplementedException();
        }

        public IGroupFilterRoot<IQueryBuilderRoute> WhereAnyHopVertex()
        {
            throw new System.NotImplementedException();
        }

        public IQueryBuilderRoute WithEdgeHopLimit(long limit)
        {
            this.EdgeHopLimit = limit;
            return this;
        }

        public IQueryBuilderRoute WithVertexHopLimit(long limit)
        {
            this.VertexHopLimit = limit;
            return this;
        }
    }
}