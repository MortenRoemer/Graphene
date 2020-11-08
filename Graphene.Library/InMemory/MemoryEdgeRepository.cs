using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Graphene.InMemory.Utility;

namespace Graphene.InMemory
{
    public class MemoryEdgeRepository : IRepository<IEdge>, IObservable<CollectionChange<IEdge>>
    {
        internal MemoryEdgeRepository(MemoryGraph graph)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Edges = new SortedDictionary<ulong, MemoryEdge>();
            Observers = new List<IObserver<CollectionChange<IEdge>>>();
        }

        private MemoryGraph Graph { get; }

        private IDictionary<ulong, MemoryEdge> Edges { get; }
        
        private IList<IObserver<CollectionChange<IEdge>>> Observers { get; }

        public MemoryEdge Create(IVertex fromVertex, IVertex toVertex, bool directed)
        {
            if (!Graph.Vertices.Contains(fromVertex.Id))
                throw new ArgumentException($"{nameof(fromVertex)} with id {fromVertex.Id} does not exist in this graph");

            if (!Graph.Vertices.Contains(toVertex.Id))
                throw new ArgumentException($"{nameof(toVertex)} with id {toVertex.Id} does not exist in this graph");

            var id = Graph.TakeId();
            var result = new MemoryEdge(Graph, fromVertex, toVertex, directed, id);
            Edges.Add(id, result);
            NotifyObservers(new[] { new CollectionChange<IEdge>(result, CollectionChangeMode.Addition) });
            return result;
        }

        public void Clear()
        {
            NotifyObservers(Edges.Select(edge =>
                new CollectionChange<IEdge>(edge.Value, CollectionChangeMode.Removal)).ToArray());
            
            foreach (var (key, value) in Edges)
            {
                Graph.FreeId(key);
            }

            Edges.Clear();
        }

        public bool Contains(IEnumerable<ulong> ids)
        {
            return ids.All(id => Edges.ContainsKey(id));
        }

        public ulong Count()
        {
            return (ulong)Edges.Count;
        }

        public void Delete(IEnumerable<IEdge> items)
        {
            var enumerable = items as IEdge[] ?? items.ToArray();
            NotifyObservers(enumerable.Select(item =>
                new CollectionChange<IEdge>(item, CollectionChangeMode.Removal)).ToArray());
            
            foreach (var item in enumerable)
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

        public IDisposable Subscribe(IObserver<CollectionChange<IEdge>> observer)
        {
            Observers.Add(observer);
            return new Subscription(Observers, observer);
        }

        private void NotifyObservers(IEnumerable<CollectionChange<IEdge>> changes)
        {
            foreach (var observer in Observers)
            {
                foreach (var change in changes)
                {
                    observer.OnNext(change);
                }
                observer.OnCompleted();
            }
        }

        private class Subscription : IDisposable
        {
            public Subscription(IList<IObserver<CollectionChange<IEdge>>> observerList, IObserver<CollectionChange<IEdge>> subscriber)
            {
                ObserverList = observerList ?? throw new ArgumentNullException(nameof(observerList));
                Subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            }

            ~Subscription()
            {
                Dispose();
            }
            
            private IList<IObserver<CollectionChange<IEdge>>> ObserverList { get; }
            
            private IObserver<CollectionChange<IEdge>> Subscriber { get; }
            
            private bool Disposed { get; set; }
            
            public void Dispose()
            {
                if (Disposed)
                    throw new ObjectDisposedException(nameof(Subscription));

                ObserverList.Remove(Subscriber);
                GC.SuppressFinalize(this);
                Disposed = true;
            }
        }
    }
}