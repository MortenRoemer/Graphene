using System;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class WithMinimalMetric : IWithMinimalMetric
    {
        internal WithMinimalMetric(FromVertex fromVertex, Func<IReadOnlyEdge, float> metricFunction)
        {
            FromVertex = fromVertex;
            MetricFunction = metricFunction;
        }
        
        internal FromVertex FromVertex { get; }
        
        internal Func<IReadOnlyEdge, float> MetricFunction { get; }
        
        internal Func<IReadOnlyEdge, bool> Filter { get; private set; }
        
        public IToVertex<float> ToVertex(int targetId)
        {
            return new WithMinimalMetricToVertex(this, targetId);
        }

        public IWithMinimalMetric Where(Func<IReadOnlyEdge, bool> filter)
        {
            Filter = Filter is null ? filter : edge => Filter(edge) && filter(edge);
            return this;
        }
    }
}