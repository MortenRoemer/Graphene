using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class BuilderRoute : IQueryBuilderRoute
    {
        internal BuilderRoute(BuilderRoot root, RouteSearchMode mode) : this(root, mode, null) {}

        internal BuilderRoute(BuilderRoot root, RouteSearchMode mode, IEnumerable<Guid> range)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Mode = mode;
            Range = range;
        }

        private BuilderRoot Root { get; }

        private RouteSearchMode Mode { get; }

        private IEnumerable<Guid> Range { get; }

        private long? EdgeHopLimit { get; set; }

        private long? VertexHopLimit { get; set; }

        private FilterRoot<IQueryBuilderRoute> TargetFilter { get; set; }

        private FilterRoot<IQueryBuilderRoute> EdgeFilter { get; set; }

        private FilterRoot<IQueryBuilderRoute> VertexFilter { get; set; }

        private OptimizerSettings OptimizerSettings { get; set; }

        public IQueryBuilderOptimizer<IQueryBuilderRoute> OptimizeSoThat()
        {
            return (IQueryBuilderOptimizer<IQueryBuilderRoute>)new OptimizerRoot(this);
        }

        public IQueryResult Resolve()
        {
            return Root.Resolve();
        }

        internal void SetOptimizerSettings(OptimizerSettings settings)
        {
            OptimizerSettings = settings;
        }

        public IFilterRoot<IQueryBuilderRoute> Where()
        {
            TargetFilter = new FilterRoot<IQueryBuilderRoute>(this);
            return TargetFilter;
        }

        public IFilterRoot<IQueryBuilderRoute> WhereAnyHopEdge()
        {
            EdgeFilter = new FilterRoot<IQueryBuilderRoute>(this);
            return EdgeFilter;
        }

        public IFilterRoot<IQueryBuilderRoute> WhereAnyHopVertex()
        {
            VertexFilter = new FilterRoot<IQueryBuilderRoute>(this);
            return VertexFilter;
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