using System;
using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class BuilderRoute : IQueryBuilderRoute
    {
        internal BuilderRoute(BuilderRoot root, RouteSearchMode mode) : this(root, mode, null) {}

        internal BuilderRoute(BuilderRoot root, RouteSearchMode mode, IEnumerable<ulong> range)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            Mode = mode;
            Range = range;
        }

        private BuilderRoot Root { get; }

        private RouteSearchMode Mode { get; }

        private IEnumerable<ulong> Range { get; }

        private ulong? EdgeHopLimit { get; set; }

        private ulong? VertexHopLimit { get; set; }

        private FilterRoot<IQueryBuilderRoute> TargetFilter { get; set; }

        private FilterRoot<IQueryBuilderRoute> EdgeFilter { get; set; }

        private FilterRoot<IQueryBuilderRoute> VertexFilter { get; set; }

        private OptimizerSettings OptimizerSettings { get; set; }

        public IQueryBuilderOptimizer<IQueryBuilderRoute> OptimizeSoThat()
        {
            return new OptimizerRoot(this);
        }

        public bool Resolve(out IQueryResult result)
        {
            return Root.Resolve(out result);
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

        public IQueryBuilderRoute WithEdgeHopLimit(ulong limit)
        {
            this.EdgeHopLimit = limit;
            return this;
        }

        public IQueryBuilderRoute WithVertexHopLimit(ulong limit)
        {
            this.VertexHopLimit = limit;
            return this;
        }
    }
}