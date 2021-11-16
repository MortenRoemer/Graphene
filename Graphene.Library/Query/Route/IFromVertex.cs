using System;
using System.Linq.Expressions;

namespace Graphene.Query.Route
{
    public interface IFromVertex
    {
        IWithMinimalEdges WithMinimalEdges();

        IWithMinimalMetric<TMetric> WithMinimalMetric<TMetric>(Expression<Func<IReadOnlyEdge, TMetric>> metricFunction, TMetric defaultCost, Expression<Func<TMetric, TMetric, TMetric>> accumulatorFunction) where TMetric : IComparable<TMetric>;
    }
}