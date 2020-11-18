using System;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query.Route
{
    public class FromVertex : IFromVertex
    {
        internal FromVertex(Root root, int vertexId)
        {
            Root = root;
            VertexId = vertexId;
        }
        
        internal Root Root { get; }
        
        internal int VertexId { get; }
        
        public IWithMinimalEdges WithMinimalEdges()
        {
            return new WithMinimalEdges(this);
        }

        public IWithMinimalMetric<TMetric> WithMinimalMetric<TMetric>(Func<IReadOnlyEdge, TMetric> metricFunction, Func<TMetric, TMetric, TMetric> accumulatorFunction) where TMetric : IComparable<TMetric>
        {
            if (metricFunction is null)
                throw new ArgumentNullException(nameof(metricFunction));
            
            if (accumulatorFunction is null)
                throw new ArgumentNullException(nameof(accumulatorFunction));
            
            return new WithMinimalMetric<TMetric>(this, metricFunction, accumulatorFunction);
        }
    }
}