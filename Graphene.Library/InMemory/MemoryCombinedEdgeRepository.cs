using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryCombinedEdgeRepository : IReadOnlyRepository<IEdge>, IObserver<IEdge>
    {
        internal MemoryCombinedEdgeRepository(MemoryEdgeRepository edges, MemoryVertex vertex)
        {
            Edges = edges ?? throw new ArgumentNullException(nameof(edges));
            Vertex = vertex ?? throw new ArgumentNullException(nameof(vertex));
            Subscription = edges.Subscribe(this);
        }

        ~MemoryCombinedEdgeRepository()
        {
            Subscription.Dispose();
        }

        private MemoryEdgeRepository Edges { get; }
        
        private MemoryVertex Vertex { get; }
        
        private IReadOnlyList<IEdge> Buffer { get; set; }
        
        private IDisposable Subscription { get; }

        public bool Contains(IEnumerable<ulong> ids)
        {
            return GetEdges().All(edge => ids.Contains(edge.Id));
        }

        public ulong Count()
        {
            return Edges.Count();
        }

        public IEnumerable<IEdge> Get(IEnumerable<ulong> ids)
        {
            return GetEdges().Where(edge => ids.Contains(edge.Id));
        }

        public IEnumerator<IEdge> GetEnumerator()
        {
            return GetEdges().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<IEdge> GetEdges()
        {
            return Buffer ??= Edges.Where(IsContainedEdge).ToArray();
        }

        private bool IsContainedEdge(IEdge edge)
        {
            return edge.FromVertex.Id == Vertex.Id || edge.ToVertex.Id == Vertex.Id;
        }

        public void OnCompleted()
        {
            // do nothing
        }

        public void OnError(Exception error)
        {
            // do nothing
        }

        public void OnNext(IEdge value)
        {
            if (IsContainedEdge(value))
                Buffer = null;
        }
    }
}