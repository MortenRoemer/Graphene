namespace Graphene.InMemory {
    public class MemoryGraph : IGraph
    {
        public MemoryGraph()
        {
        }

        public long Size => throw new System.NotImplementedException();

        public IRepository<IVertex> Vertices => throw new System.NotImplementedException();

        public IRepository<IEdge> Edges => throw new System.NotImplementedException();

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}