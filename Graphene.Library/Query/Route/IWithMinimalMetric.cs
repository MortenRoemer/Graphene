using System;
using System.Linq.Expressions;

namespace Graphene.Query.Route
{
    public interface IWithMinimalMetric<TMetric> where TMetric : IComparable<TMetric>
    {
        IToVertex<TMetric> ToVertex(Guid toVertex);

        IWithMinimalMetric<TMetric> Where(Expression<Func<IReadOnlyEdge, bool>> filter);

        IWithMinimalMetric<TMetric> WithHeuristic(Expression<Func<IReadOnlyVertex, IReadOnlyVertex, TMetric>> heuristicFunction);
    }
}