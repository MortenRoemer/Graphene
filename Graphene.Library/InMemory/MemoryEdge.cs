using System;

namespace Graphene.InMemory
{
    public class MemoryEdge : IEdge
    {
        internal MemoryEdge(MemoryGraph graph, IVertex fromVertex, IVertex toVertex, bool directed, int id)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            FromVertex = fromVertex ?? throw new ArgumentNullException(nameof(fromVertex));
            ToVertex = toVertex ?? throw new ArgumentNullException(nameof(toVertex));
            Directed = directed;
            Id = id;
            Attributes = new MemoryAttributeSet();
            Vertices = new MemoryCombinedVertexRepository(this);
        }

        IReadOnlyRepository<IReadOnlyVertex> IReadOnlyEdge.Vertices => Vertices;

        IReadOnlyVertex IReadOnlyEdge.FromVertex => FromVertex;

        IReadOnlyVertex IReadOnlyEdge.ToVertex => ToVertex;

        public IVertex FromVertex { get; }

        public IVertex ToVertex { get; }

        public bool Directed { get; }

        public IGraph Graph { get; }

        IReadOnlyGraph IReadOnlyEntity.Graph => Graph;

        public int Id { get; }

        public string Label { get; set; }
        IReadOnlyAttributeSet IReadOnlyEntity.Attributes => Attributes;

        public IAttributeSet Attributes { get; }

        public IReadOnlyRepository<IVertex> Vertices { get; }

        public override bool Equals(object obj)
        {
            return obj is IEdge other && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public IEdge Promote()
        {
            return this;
        }

        public bool TryPromote(out IEdge target)
        {
            target = this;
            return true;
        }
    }
}