using System;

namespace Graphene.InMemory
{
    public class MemoryVertex : IVertex
    {
        public MemoryVertex(MemoryGraph graph, Guid id)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Id = id;
            Attributes = new MemoryAttributeSet();
        }

        public IVertexEdgeRepository IngoingEdges => throw new NotImplementedException();

        public IVertexEdgeRepository OutgoingEdges => throw new NotImplementedException();

        public IVertexEdgeRepository BidirectionalEdges => throw new NotImplementedException();

        public IGraph Graph { get; }

        public Guid Id { get; }

        public string Label { get; set; }

        public IAttributeSet Attributes { get; }
    }
}