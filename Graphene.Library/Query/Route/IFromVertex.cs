using System;

namespace Graphene.Query.Route
{
    public interface IFromVertex
    {
        IWithMinimalEdges WithMinimalEdges();

        IWithMinimalMetric WithMinimalMetric(Func<IReadOnlyEdge, float> metricFunction);
    }
}