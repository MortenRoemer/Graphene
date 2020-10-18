using System;

namespace Graphene.InMemory
{
    public class MemoryEdge : IEdge
    {
        internal MemoryEdge(MemoryGraph graph, IVertex fromVertex, IVertex toVertex, bool directed, ulong id)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            FromVertex = fromVertex ?? throw new ArgumentNullException(nameof(fromVertex));
            ToVertex = toVertex ?? throw new ArgumentNullException(nameof(toVertex));
            Directed = directed;
            Id = id;
            Attributes = new MemoryAttributeSet();
            Vertices = new MemoryCombinedVertexRepository(this);
        }

        public IVertex FromVertex { get; }

        public IVertex ToVertex { get; }

        public bool Directed { get; }

        public IGraph Graph { get; }

        public ulong Id { get; }

        public string Label { get; set; }

        public IAttributeSet Attributes { get; }

        public IReadOnlyRepository<IVertex> Vertices { get; }
    }
}