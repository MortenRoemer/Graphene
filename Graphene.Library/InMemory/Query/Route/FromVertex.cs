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

        public IWithMinimalMetric WithMinimalMetric(Func<IReadOnlyEdge, float> metricFunction)
        {
            if (metricFunction is null)
                throw new ArgumentNullException(nameof(metricFunction));
            
            return new WithMinimalMetric(this, metricFunction);
        }
    }
}