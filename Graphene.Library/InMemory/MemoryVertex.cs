using System;

namespace Graphene.InMemory
{
    public class MemoryVertex : IVertex
    {
        internal MemoryVertex(MemoryGraph graph, MemoryEdgeRepository edges, int id, string label)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Id = id;
            Label = label;
            Attributes = new MemoryAttributeSet(graph);
            IngoingEdges = new MemoryRelativeEdgeRepository.Ingoing(edges, this);
            OutgoingEdges = new MemoryRelativeEdgeRepository.Outgoing(edges, this);
            BidirectionalEdges = new MemoryRelativeEdgeRepository.Bidirectional(edges, this);
            Edges = new MemoryReadOnlyEdgeRepository.Combined(edges, this);
        }

        IReadOnlyRepository<IReadOnlyEdge> IReadOnlyVertex.Edges => Edges;

        IReadOnlyRepository<IReadOnlyEdge> IReadOnlyVertex.IngoingEdges => IngoingEdges;

        IReadOnlyRepository<IReadOnlyEdge> IReadOnlyVertex.OutgoingEdges => OutgoingEdges;

        IReadOnlyRepository<IReadOnlyEdge> IReadOnlyVertex.BidirectionalEdges => BidirectionalEdges;

        public IVertexEdgeRepository IngoingEdges { get; }

        public IVertexEdgeRepository OutgoingEdges { get; }

        public IVertexEdgeRepository BidirectionalEdges { get; }

        public IGraph Graph { get; }

        IReadOnlyGraph IReadOnlyEntity.Graph => Graph;

        public int Id { get; }

        public string Label { get; }
        
        IReadOnlyAttributeSet IReadOnlyEntity.Attributes => Attributes;

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
        
        public IVertex Promote()
        {
            return this;
        }

        public bool TryPromote(out IVertex target)
        {
            target = this;
            return true;
        }

        public static bool operator ==(MemoryVertex left, MemoryVertex right) => left?.Id == right?.Id;
        public static bool operator !=(MemoryVertex left, MemoryVertex right) => !(left == right);
    }
}