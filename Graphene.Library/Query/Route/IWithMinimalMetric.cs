using System;

namespace Graphene.Query.Route
{
    public interface IWithMinimalMetric<TMetric> where TMetric : IComparable<TMetric>
    {
        IToVertex<TMetric> ToVertex(int index);

        IWithMinimalMetric<TMetric> Where(Func<IReadOnlyEdge, bool> filter);

        IWithMinimalMetric<TMetric> WithHeuristic(Func<IReadOnlyVertex, IReadOnlyVertex, TMetric> heuristicFunction);
    }
}