using System;
using System.Linq.Expressions;
using Graphene.Query.Route;

namespace Graphene.InMemory.Query
{
    internal class FromVertex : IFromVertex
    {
        public FromVertex(MemoryGraph graph, Guid fromVertexId)
        {
            Graph = graph;
            FromVertexId = fromVertexId;
        }

        private MemoryGraph Graph { get; }
        
        private Guid FromVertexId { get; }
        
        public IWithMinimalEdges WithMinimalEdges()
        {
            return new WithMinimalEdges(Graph, FromVertexId);
        }

        public IWithMinimalMetric<TMetric> WithMinimalMetric<TMetric>(Expression<Func<IReadOnlyEdge, TMetric>> metricFunction, TMetric defaultCost, Expression<Func<TMetric, TMetric, TMetric>> accumulatorFunction) where TMetric : IComparable<TMetric>
        {
            return new WithMinimalMetric<TMetric>(
                Graph, 
                FromVertexId, 
                metricFunction.Compile(),
                defaultCost,
                accumulatorFunction.Compile());
        }
    }
}