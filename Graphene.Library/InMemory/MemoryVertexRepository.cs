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
            Vertices = new SortedDictionary<int, MemoryVertex>();
            Edges = edges ?? throw new ArgumentNullException(nameof(edges));
        }

        private MemoryGraph Graph { get; }

        private IDictionary<int, MemoryVertex> Vertices { get; }

        private MemoryEdgeRepository Edges { get; }

        public void Clear()
        {
            foreach(var vertex in Vertices)
                Graph.FreeId(vertex.Key);

            Vertices.Clear();
            Edges.Clear();
        }

        public bool Contains(IEnumerable<int> ids)
        {
            return ids.All(id => Vertices.ContainsKey(id));
        }

        public bool Contains(int id)
        {
            return Vertices.ContainsKey(id);
        }

        public int Count()
        {
            return Vertices.Count;
        }

        public IVertex Create()
        {
            var vertex = new MemoryVertex(Graph, Edges, Graph.TakeId());
            Vertices.Add(vertex.Id, vertex); 
            return vertex;
        }

        public IVertex Create(string label)
        {
            var vertex = new MemoryVertex(Graph, Edges, Graph.TakeId());
            Vertices.Add(vertex.Id, vertex);
            vertex.Label = label;
            return vertex;
        }

        public void Delete(IEnumerable<int> ids)
        {
            foreach (var id in ids)
            {
                Delete(id);
            }
        }
        
        public void Delete(int id)
        {
            Vertices.Remove(id);
            Graph.FreeId(id);
        }

        public IEnumerable<IVertex> Get(IEnumerable<int> ids)
        {
            return ids.Select(Get);
        }
        
        public IVertex Get(int id)
        {
            return Vertices[id];
        }

        public IEnumerator<IVertex> GetEnumerator()
        {
            return Vertices.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}