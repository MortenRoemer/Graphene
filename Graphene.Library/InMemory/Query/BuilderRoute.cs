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

        internal RouteSearchMode Mode { get; }

        internal IEnumerable<ulong> Range { get; }

        internal ulong? EdgeHopLimit { get; private set; }

        internal ulong? VertexHopLimit { get; private set; }

        internal FilterRoot<IQueryBuilderRoute> TargetFilter { get; private set; }

        internal FilterRoot<IQueryBuilderRoute> EdgeFilter { get; private set; }

        internal FilterRoot<IQueryBuilderRoute> VertexFilter { get; private set; }

        internal OptimizerSettings OptimizerSettings { get; private set; }

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