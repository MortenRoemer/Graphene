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

        public bool Contains(IEnumerable<int>? ids)
            => ids is null || ids.All(Contains);

        public bool Contains(int id)
            => id == Edge.FromVertex.Id || id == Edge.ToVertex.Id;

        public int Count()
            => Vertices.Count();

        public IEnumerable<IVertex> Get(IEnumerable<int>? ids)
            => Vertices.Where(vertex => ids.Contains(vertex.Id));

        public IVertex? Get(int id)
            => Vertices.FirstOrDefault(vertex => vertex.Id == id);
        
        public IEnumerator<IVertex> GetEnumerator()
            => Vertices.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}