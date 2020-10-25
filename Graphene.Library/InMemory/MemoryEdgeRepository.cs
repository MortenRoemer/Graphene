using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryEdgeRepository : IRepository<IEdge>
    {
        internal MemoryEdgeRepository(MemoryGraph graph)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Edges = new SortedDictionary<ulong, MemoryEdge>();
        }

        private MemoryGraph Graph { get; }

        private IDictionary<ulong, MemoryEdge> Edges { get; }

        public MemoryEdge Create(IVertex fromVertex, IVertex toVertex, bool directed)
        {
            if (!Graph.Vertices.Contains(fromVertex.Id))
                throw new ArgumentException($"{nameof(fromVertex)} with id {fromVertex.Id} does not exist in this graph");

            if (!Graph.Vertices.Contains(toVertex.Id))
                throw new ArgumentException($"{nameof(toVertex)} with id {toVertex.Id} does not exist in this graph");

            var id = Graph.TakeId();
            var result = new MemoryEdge(Graph, fromVertex, toVertex, directed, id);
            Edges.Add(id, result);
            return result;
        }

        public void Clear()
        {
            foreach (var edge in Edges)
                Graph.FreeId(edge.Key);

            Edges.Clear();
        }

        public bool Contains(IEnumerable<ulong> ids)
        {
            return ids.All(id => Edges.ContainsKey(id));
        }

        public long Count()
        {
            return Edges.Count;
        }

        public void Delete(IEnumerable<IEdge> items)
        {
            foreach (var item in items)
            {
                Edges.Remove(item.Id);
                Graph.FreeId(item.Id);
            }
        }

        public IEnumerable<IEdge> Get(IEnumerable<ulong> ids)
        {
            return ids.Select(id => Edges[id]);
        }

        public IEnumerator<IEdge> GetEnumerator()
        {
            return Edges.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}