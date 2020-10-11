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
            Vertices = new SortedDictionary<ulong, MemoryVertex>();
            Edges = edges ?? throw new ArgumentNullException(nameof(edges));
        }

        private MemoryGraph Graph { get; }

        private IDictionary<ulong, MemoryVertex> Vertices { get; }

        private MemoryEdgeRepository Edges { get; }

        private static Random Random { get; } = new Random();

        public void Clear()
        {
            Vertices.Clear();
            Edges.Clear();
        }

        public bool Contains(IEnumerable<ulong> ids)
        {
            return ids.All(id => Vertices.ContainsKey(id));
        }

        public long Count()
        {
            return Vertices.Count;
        }

        public IVertex Create()
        {
            var vertex = new MemoryVertex(Graph, Edges, GenerateUniqueId());
            Vertices.Add(vertex.Id, vertex); 
            return vertex;
        }

        public void Delete(IEnumerable<IVertex> items)
        {
            foreach (var item in items)
            {
                Vertices.Remove(item.Id);
            }
        }

        public IEnumerable<IVertex> Get(IEnumerable<ulong> ids)
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

        private ulong GenerateUniqueId()
        {
            byte[] buffer = new byte[8];

            while (true)
            {
                Random.NextBytes(buffer);
                var id = BitConverter.ToUInt64(buffer);

                if (!Vertices.ContainsKey(id))
                    return id;
            }
        }
    }
}