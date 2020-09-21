using System;

namespace Graphene.InMemory
{
    public class MemoryEdge : IEdge
    {
        public MemoryEdge(MemoryGraph graph, IVertex fromVertex, IVertex toVertex, bool directed, Guid id)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            FromVertex = fromVertex ?? throw new ArgumentNullException(nameof(fromVertex));
            ToVertex = toVertex ?? throw new ArgumentNullException(nameof(toVertex));
            Directed = directed;
            Id = id;
            Attributes = new MemoryAttributeSet();
        }

        public IVertex FromVertex { get; }

        public IVertex ToVertex { get; }

        public bool Directed { get; }

        public IGraph Graph { get; }

        public Guid Id { get; }

        public string Label { get; set; }

        public IAttributeSet Attributes { get; }
    }
}