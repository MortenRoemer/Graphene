namespace Graphene.InMemory {
    public class MemoryGraph : IGraph
    {
        public MemoryGraph()
        {
            MemoryEdges = new MemoryEdgeRepository(this);
            MemoryVertices = new MemoryVertexRepository(this, MemoryEdges);
        }

        public long Size => Vertices.Count() + Edges.Count();

        public IVertexRepository Vertices => MemoryVertices;

        public IReadOnlyRepository<IEdge> Edges => MemoryEdges;

        private MemoryVertexRepository MemoryVertices { get; }

        private MemoryEdgeRepository MemoryEdges { get; }

        public void Clear()
        {
            MemoryEdges.Clear();
            Vertices.Clear();
        }

        public IGraph Clone()
        {
            throw new System.NotImplementedException();
        }

        public void Merge(IGraph other)
        {
            throw new System.NotImplementedException();
        }
    }
}