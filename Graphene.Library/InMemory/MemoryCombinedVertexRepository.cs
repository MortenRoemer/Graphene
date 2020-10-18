using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryCombinedVertexRepository : IReadOnlyRepository<IVertex>
    {
        internal MemoryCombinedVertexRepository(IEdge edge)
        {
            Edge = edge ?? throw new ArgumentNullException(nameof(edge));
            Vertices = Edge.FromVertex.Id == Edge.ToVertex.Id
                ? new[] { Edge.FromVertex }
                : new[] { Edge.FromVertex, Edge.ToVertex };
        }

        private IEdge Edge { get; }

        private IEnumerable<IVertex> Vertices { get; }

        public bool Contains(IEnumerable<ulong> ids)
            => ids.All(id => id == Edge.FromVertex.Id || id == Edge.ToVertex.Id);

        public long Count()
            => Vertices.Count();

        public IEnumerable<IVertex> Get(IEnumerable<ulong> ids)
            => Vertices.Where(vertex => ids.Contains(vertex.Id));

        public IEnumerator<IVertex> GetEnumerator()
            => Vertices.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}