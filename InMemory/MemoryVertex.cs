using System;

namespace Graphene.InMemory
{
    public class MemoryVertex : IVertex
    {
        internal MemoryVertex(MemoryGraph graph, MemoryEdgeRepository edges, Guid id)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Id = id;
            Attributes = new MemoryAttributeSet();
            IngoingEdges = new MemoryIngoingEdgeRepository(edges, this);
            OutgoingEdges = new MemoryOutgoingEdgeRepository(edges, this);
            BidirectionalEdges = new MemoryBidirectionalEdgeRepository(edges, this);
        }

        public IVertexEdgeRepository IngoingEdges { get; }

        public IVertexEdgeRepository OutgoingEdges { get; }

        public IVertexEdgeRepository BidirectionalEdges { get; }

        public IGraph Graph { get; }

        public Guid Id { get; }

        public string Label { get; set; }

        public IAttributeSet Attributes { get; }
    }
}