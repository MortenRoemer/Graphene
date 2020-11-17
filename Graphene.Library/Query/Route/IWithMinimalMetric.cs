using System;

namespace Graphene.Query.Route
{
    public interface IWithMinimalMetric
    {
        IToVertex<float> ToVertex(int index);

        IWithMinimalMetric Where(Func<IReadOnlyEdge, bool> filter);
    }
}