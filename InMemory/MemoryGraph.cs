namespace Graphene.InMemory {
    public class MemoryGraph : IGraph
    {
        public MemoryGraph()
        {
            Vertices = new MemoryVertexRepository(this);
        }

        public long Size => throw new System.NotImplementedException();

        public IVertexRepository Vertices  { get; }

        public IReadOnlyRepository<IEdge> Edges => throw new System.NotImplementedException();

        public void Clear()
        {
            throw new System.NotImplementedException();
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