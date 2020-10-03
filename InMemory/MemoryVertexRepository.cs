using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryVertexRepository : IVertexRepository
    {
        internal MemoryVertexRepository(MemoryGraph graph, MemoryEdgeRepository edges)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Vertices = new SortedDictionary<long, MemoryVertex>();
            Edges = edges ?? throw new ArgumentNullException(nameof(edges));
        }

        private MemoryGraph Graph { get; }

        private IDictionary<long, MemoryVertex> Vertices { get; }

        private MemoryEdgeRepository Edges { get; }

        private static Random Random { get; } = new Random();

        public void Clear()
        {
            Vertices.Clear();
            Edges.Clear();
        }

        public bool Contains(IEnumerable<long> ids)
        {
            return ids.All(id => Vertices.ContainsKey(id));
        }

        public long Count()
        {
            return Vertices.Count;
        }

        public IVertex Create()
        {
            return new MemoryVertex(Graph, Edges, GenerateUniqueId());
        }

        public void Delete(IEnumerable<IVertex> items)
        {
            foreach (var item in items)
            {
                Vertices.Remove(item.Id);
            }
        }

        public IEnumerable<IVertex> Get(IEnumerable<long> ids)
        {
            return ids.Select(id => Vertices[id]);
        }

        public IEnumerator<IVertex> GetEnumerator()
        {
            return Vertices.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private long GenerateUniqueId()
        {
            byte[] buffer = new byte[8];

            while (true)
            {
                Random.NextBytes(buffer);
                var id = BitConverter.ToInt64(buffer);

                if (!Vertices.ContainsKey(id))
                    return id;
            }
        }
    }
}