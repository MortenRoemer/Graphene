using System.Collections.Generic;
using Graphene.Query;

namespace Graphene.InMemory.Query
{
    public class MemoryGraphView : IReadOnlyGraph
    {
        internal MemoryGraphView(IReadOnlyGraph backend, IEnumerable<int> vertexRange, IEnumerable<int> edgeRange)
        {
            Backend = backend;
            Vertices = new MemoryVertexView(backend.Vertices, vertexRange);
            Edges = new MemoryEdgeView(backend.Edges, edgeRange);
        }
        
        private IReadOnlyGraph Backend { get; }
        
        public int Size => Vertices.Count() + Edges.Count();
        
        public IReadOnlyRepository<IReadOnlyVertex> Vertices { get; }
        
        public IReadOnlyRepository<IReadOnlyEdge> Edges { get; }
        
        public IGraph Clone()
        {
            var newGraph = new MemoryGraph();
            newGraph.Merge(Backend);
            return newGraph;
        }

        public IQueryRoot Select()
        {
            return new QueryRoot(Backend);
        }
    }
}