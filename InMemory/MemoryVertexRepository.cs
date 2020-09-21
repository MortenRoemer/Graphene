using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryVertexRepository : IVertexRepository
    {
        private MemoryGraph Graph { get; }

        private IDictionary<Guid, MemoryVertex> Vertices { get; }

        public MemoryVertexRepository(MemoryGraph graph)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Vertices = new SortedDictionary<Guid, MemoryVertex>();
        }

        public void Clear()
        {
            Vertices.Clear();
            // TODO: #1 Also clear edges
        }

        public bool Contains(IEnumerable<Guid> ids)
        {
            return ids.All(id => Vertices.ContainsKey(id));
        }

        public long Count()
        {
            return Vertices.Count;
        }

        public IVertex Create()
        {
            return new MemoryVertex(Graph, GenerateUniqueId());
        }

        public void Delete(IEnumerable<IVertex> items)
        {
            foreach (var item in items)
            {
                Vertices.Remove(item.Id);
            }
        }

        public IEnumerable<IVertex> Get(IEnumerable<Guid> ids)
        {
            return ids.Select(id => Vertices[id]);
        }

        public IEnumerator<IVertex> GetEnumerator()
        {
            return Vertices.Values.GetEnumerator();
        }

        public void Update(IEnumerable<IVertex> items)
        {
            // do nothing
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Guid GenerateUniqueId()
        {
            while (true)
            {
                var id = Guid.NewGuid();

                if (!Vertices.ContainsKey(id))
                    return id;
            }
        }
    }
}