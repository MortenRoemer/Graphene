using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryOutgoingEdgeRepository : IVertexEdgeRepository
    {
        public MemoryOutgoingEdgeRepository(MemoryEdgeRepository edges, MemoryVertex vertex)
        {
            Edges = edges ?? throw new ArgumentNullException(nameof(edges));
            Vertex = vertex ?? throw new ArgumentNullException(nameof(vertex));
        }

        private MemoryEdgeRepository Edges { get; }

        private MemoryVertex Vertex { get; }

        public IEdge Add(IVertex other)
        {
            return Edges.Create(Vertex, other, directed: true);
        }

        public void Clear()
        {
            Edges.Delete(FilterEdges().ToArray());
        }

        public bool Contains(IEnumerable<Guid> ids)
        {
            return Edges.Get(ids).All(edge => IsContainedEdge(edge));
        }

        public long Count()
        {
            return FilterEdges().LongCount();
        }

        public void Delete(IEdge edge)
        {
            Edges.Delete(edge);
        }

        public IEnumerable<IEdge> Get(IEnumerable<Guid> ids)
        {
            return Edges
                .Get(ids)
                .Where(edge => IsContainedEdge(edge));
        }

        public IEnumerator<IEdge> GetEnumerator()
        {
            return FilterEdges().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<IEdge> FilterEdges()
        {
            return Edges
                .Where(edge => IsContainedEdge(edge));
        }

        private bool IsContainedEdge(IEdge edge)
        {
            return edge.Directed && edge.FromVertex.Id == Vertex.Id;
        }
    }
}