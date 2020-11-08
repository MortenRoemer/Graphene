using System;

namespace Graphene.InMemory
{
    public class MemoryVertex : IVertex
    {
        internal MemoryVertex(MemoryGraph graph, MemoryEdgeRepository edges, ulong id)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Id = id;
            Attributes = new MemoryAttributeSet();
            IngoingEdges = new MemoryRelativeEdgeRepository.Ingoing(edges, this);
            OutgoingEdges = new MemoryRelativeEdgeRepository.Outgoing(edges, this);
            BidirectionalEdges = new MemoryRelativeEdgeRepository.Bidirectional(edges, this);
            Edges = new MemoryReadOnlyEdgeRepository.Combined(edges, this);
        }

        public IVertexEdgeRepository IngoingEdges { get; }

        public IVertexEdgeRepository OutgoingEdges { get; }

        public IVertexEdgeRepository BidirectionalEdges { get; }

        public IGraph Graph { get; }

        public ulong Id { get; }

        public string Label { get; set; }

        public IAttributeSet Attributes { get; }
        
        public IReadOnlyRepository<IEdge> Edges { get; }

        public override bool Equals(object obj)
        {
            return obj is IVertex other && this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(MemoryVertex left, MemoryVertex right) => left?.Id == right?.Id;
        public static bool operator !=(MemoryVertex left, MemoryVertex right) => !(left == right);
    }
}