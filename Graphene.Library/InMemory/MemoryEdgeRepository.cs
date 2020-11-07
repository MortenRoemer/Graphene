using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public class MemoryEdgeRepository : IRepository<IEdge>, IObservable<IEdge>
    {
        internal MemoryEdgeRepository(MemoryGraph graph)
        {
            Graph = graph ?? throw new ArgumentNullException(nameof(graph));
            Edges = new SortedDictionary<ulong, MemoryEdge>();
            Observers = new List<IObserver<IEdge>>();
        }

        private MemoryGraph Graph { get; }

        private IDictionary<ulong, MemoryEdge> Edges { get; }
        
        private IList<IObserver<IEdge>> Observers { get; }

        public MemoryEdge Create(IVertex fromVertex, IVertex toVertex, bool directed)
        {
            if (!Graph.Vertices.Contains(fromVertex.Id))
                throw new ArgumentException($"{nameof(fromVertex)} with id {fromVertex.Id} does not exist in this graph");

            if (!Graph.Vertices.Contains(toVertex.Id))
                throw new ArgumentException($"{nameof(toVertex)} with id {toVertex.Id} does not exist in this graph");

            var id = Graph.TakeId();
            var result = new MemoryEdge(Graph, fromVertex, toVertex, directed, id);
            Edges.Add(id, result);
            NotifyObservers(result);
            return result;
        }

        public void Clear()
        {
            foreach (var (key, value) in Edges)
            {
                Graph.FreeId(key);
                NotifyObservers(value);
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
            foreach (var item in items)
            {
                Edges.Remove(item.Id);
                Graph.FreeId(item.Id);
                NotifyObservers(item);
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

        public IDisposable Subscribe(IObserver<IEdge> observer)
        {
            Observers.Add(observer);
            return new Subscription(Observers, observer);
        }

        private void NotifyObservers(IEdge edge)
        {
            foreach (var observer in Observers)
            {
                observer.OnNext(edge);
                observer.OnCompleted();
            }
        }

        private class Subscription : IDisposable
        {
            public Subscription(IList<IObserver<IEdge>> observerList, IObserver<IEdge> subscriber)
            {
                ObserverList = observerList ?? throw new ArgumentNullException(nameof(observerList));
                Subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
            }

            ~Subscription()
            {
                Dispose();
            }
            
            private IList<IObserver<IEdge>> ObserverList { get; }
            
            private IObserver<IEdge> Subscriber { get; }
            
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