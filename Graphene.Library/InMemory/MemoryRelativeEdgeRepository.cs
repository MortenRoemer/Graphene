using System.Collections.Generic;

namespace Graphene.InMemory
{
    public abstract class MemoryRelativeEdgeRepository : MemoryReadOnlyEdgeRepository, IVertexEdgeRepository
    {
        private MemoryRelativeEdgeRepository(MemoryEdgeRepository edges, MemoryVertex vertex) : base(edges, vertex)
        {
        }

        public abstract IEdge Add(IVertex other, string? label = null);
        
        public void Delete(IEnumerable<int>? ids)
        {
            Edges.Delete(ids);
        }

        public void Delete(int id)
        {
            Edges.Delete(id);
        }

        public void Clear()
        {
            Edges.Delete(EdgeCache.Value.Buffer.Values);
        }

        public sealed class Ingoing : MemoryRelativeEdgeRepository
        {
            internal Ingoing(MemoryEdgeRepository edges, MemoryVertex vertex) : base(edges, vertex)
            {
            }

            public override IEdge Add(IVertex other, string? label = default)
            {
                var edge = Edges.Create(other, Vertex, directed: true, label);
                return edge;
            }

            protected override bool IsContainedEdge(IEdge edge)
            {
                return edge.Directed && edge.ToVertex.Id == Vertex.Id;
            }
        }
        
        public sealed class Outgoing : MemoryRelativeEdgeRepository
        {
            internal Outgoing(MemoryEdgeRepository edges, MemoryVertex vertex) : base(edges, vertex)
            {
            }

            public override IEdge Add(IVertex other, string? label = null)
            {
                var edge = Edges.Create(Vertex, other, directed: true, label);
                return edge;
            }

            protected override bool IsContainedEdge(IEdge edge)
            {
                return edge.Directed && edge.FromVertex.Id == Vertex.Id;
            }
        }
        
        public sealed class Bidirectional : MemoryRelativeEdgeRepository
        {
            internal Bidirectional(MemoryEdgeRepository edges, MemoryVertex vertex) : base(edges, vertex)
            {
            }

            public override IEdge Add(IVertex other, string? label = null)
            {
                var edge = Edges.Create(Vertex, other, directed: false, label);
                return edge;
            }

            protected override bool IsContainedEdge(IEdge edge)
            {
                return !edge.Directed && (edge.FromVertex.Id == Vertex.Id || edge.ToVertex.Id == Vertex.Id);
            }
        }
    }
}