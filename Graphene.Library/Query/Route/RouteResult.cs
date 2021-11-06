using System.Collections.Generic;

namespace Graphene.Query.Route
{
    public readonly struct RouteResult<TMetric>
    {
        public RouteResult(bool found, IReadOnlyVertex origin, IReadOnlyList<RouteStep> steps, TMetric metric)
        {
            Found = found;
            Origin = origin;
            Steps = steps;
            Metric = metric;
        }
        
        public bool Found { get; }
        
        public IReadOnlyVertex Origin { get; }
        
        public IReadOnlyList<RouteStep> Steps { get; }

        public TMetric Metric { get; }
    }
}