using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryCombinedEdgeRepository : IReadOnlyRepository<IEdge>
    {
        internal MemoryCombinedEdgeRepository(MemoryEdgeRepository edges, MemoryVertex vertex)
        {
            if (edges is null)
                throw new ArgumentNullException(nameof(edges));

            if (vertex is null)
                throw new ArgumentNullException(nameof(vertex));

            Edges = edges
                .Where(edge => edge.FromVertex.Id == vertex.Id || edge.ToVertex.Id == vertex.Id);
        }

        private IEnumerable<IEdge> Edges { get; }

        public bool Contains(IEnumerable<ulong> ids)
            => ids.All(id => Edges.Select(edge => edge.Id).Contains(id));

        public long Count()
            => Edges.Count();

        public IEnumerable<IEdge> Get(IEnumerable<ulong> ids)
            => Edges.Where(edge => ids.Contains(edge.Id));

        public IEnumerator<IEdge> GetEnumerator()
            => Edges.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}