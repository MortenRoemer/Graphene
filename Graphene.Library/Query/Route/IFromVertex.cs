using System;

namespace Graphene.Query.Route
{
    public interface IFromVertex
    {
        IWithMinimalEdges WithMinimalEdges();

        IWithMinimalMetric<TMetric> WithMinimalMetric<TMetric>(Func<IReadOnlyEdge, TMetric> metricFunction, Func<TMetric, TMetric, TMetric> accumulatorFunction) where TMetric : IComparable<TMetric>;
    }
}