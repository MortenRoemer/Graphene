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
            Edges = new SortedDictionary<int, MemoryEdge>();
            Observers = new List<IObserver<CollectionChange<IEdge>>>();
        }

        private MemoryGraph Graph { get; }

        private IDictionary<int, MemoryEdge> Edges { get; }
        
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
            
            foreach (var key in Edges.Keys)
            {
                Graph.FreeId(key);
            }

            Edges.Clear();
        }

        public bool Contains(IEnumerable<int> ids)
        {
            return ids.All(Contains);
        }

        public bool Contains(int id)
        {
            return Edges.ContainsKey(id);
        }

        public int Count()
        {
            return Edges.Count;
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
            NotifyObservers(new CollectionChange<IEdge>(Get(id), CollectionChangeMode.Removal));
            Edges.Remove(id);
            Graph.FreeId(id);
        }

        public IEnumerable<IEdge> Get(IEnumerable<int> ids)
        {
            return ids.Select(Get);
        }
        
        public IEdge Get(int id)
        {
            return Edges[id];
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
            foreach (var change in changes)
            {
                NotifyObservers(change);
            }
        }
        
        private void NotifyObservers(CollectionChange<IEdge> change)
        {
            foreach (var observer in Observers)
            {
                observer.OnNext(change);
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