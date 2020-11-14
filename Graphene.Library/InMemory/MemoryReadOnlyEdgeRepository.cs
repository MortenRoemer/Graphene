using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Graphene.InMemory.Utility;

namespace Graphene.InMemory
{
    public abstract class MemoryReadOnlyEdgeRepository : IReadOnlyRepository<IEdge>
    {
        protected MemoryReadOnlyEdgeRepository(MemoryEdgeRepository edges, MemoryVertex vertex)
        {
            Edges = edges ?? throw new ArgumentNullException(nameof(edges));
            Vertex = vertex ?? throw new ArgumentNullException(nameof(vertex));
            EdgeCache = new Lazy<Cache>(() => new Cache(edges, IsContainedEdge), isThreadSafe: false);
        }

        protected MemoryEdgeRepository Edges { get; }

        protected MemoryVertex Vertex { get; }
        
        protected Lazy<Cache> EdgeCache { get; }

        public bool Contains(IEnumerable<int> ids)
        {
            return ids.All(id => EdgeCache.Value.Buffer.ContainsKey(id));
        }

        public int Count()
        {
            return EdgeCache.Value.Buffer.Count;
        }

        public IEnumerable<IEdge> Get(IEnumerable<int> ids)
        {
            return ids.Select(id => EdgeCache.Value.Buffer[id]);
        }

        public IEnumerator<IEdge> GetEnumerator()
        {
            return EdgeCache.Value.Buffer.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected abstract bool IsContainedEdge(IEdge edge);

        public sealed class Combined : MemoryReadOnlyEdgeRepository
        {
            public Combined(MemoryEdgeRepository edges, MemoryVertex vertex) : base(edges, vertex)
            {
            }

            protected override bool IsContainedEdge(IEdge edge)
            {
                return edge.FromVertex.Id == Vertex.Id || edge.ToVertex.Id == Vertex.Id;
            }
        }

        protected class Cache : IObserver<CollectionChange<IEdge>>
        {
            public Cache(MemoryEdgeRepository edges, Func<IEdge, bool> predicate)
            {
                Buffer = new Dictionary<int, IEdge>();
                Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
                PopulateBuffer(edges);
                Subscription = edges.Subscribe(this);
            }

            ~Cache()
            {
                Subscription.Dispose();
            }
            
            public IDictionary<int, IEdge> Buffer { get; }

            private Func<IEdge, bool> Predicate { get; }
            
            private IDisposable Subscription { get; }

            private void PopulateBuffer(MemoryEdgeRepository edges)
            {
                foreach (var edge in edges.Where(edge => Predicate.Invoke(edge)))
                {
                    Buffer[edge.Id] = edge;
                }
            }
            
            public void OnCompleted()
            {
                // do nothing
            }

            public void OnError(Exception error)
            {
                // do nothing
            }

            public void OnNext(CollectionChange<IEdge> change)
            {
                if (!Predicate.Invoke(change.Value))
                    return;

                if (change.Mode == CollectionChangeMode.Addition)
                    Buffer[change.Value.Id] = change.Value;
                else if (change.Mode == CollectionChangeMode.Removal)
                    Buffer.Remove(change.Value.Id);
                else
                    throw new NotImplementedException($"change mode {change.Mode} is not implemented");
            }
        }
    }
}