using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Graphene.InMemory
{
    public abstract class MemoryRelativeEdgeRepository : IVertexEdgeRepository, IObserver<IEdge>
    {
        private MemoryRelativeEdgeRepository(MemoryEdgeRepository edges, MemoryVertex vertex)
        {
            Edges = edges ?? throw new ArgumentNullException(nameof(edges));
            Vertex = vertex ?? throw new ArgumentNullException(nameof(vertex));
            Subscription = edges.Subscribe(this);
        }

        ~MemoryRelativeEdgeRepository()
        {
            Subscription.Dispose();
        }

        private MemoryEdgeRepository Edges { get; }

        private MemoryVertex Vertex { get; }
        
        private IReadOnlyList<IEdge> Buffer { get; set; }
        
        private IDisposable Subscription { get; }

        public IEdge Add(IVertex other)
        {
            return Add(other, label: null);
        }

        public abstract IEdge Add(IVertex other, string label);

        public void Clear()
        {
            Edges.Delete(GetEdges().ToArray());
        }

        public bool Contains(IEnumerable<ulong> ids)
        {
            return GetEdges().All(edge => ids.Contains(edge.Id));
        }

        public ulong Count()
        {
            return (ulong)GetEdges().LongCount();
        }

        public void Delete(IEdge edge)
        {
            Edges.Delete(edge);
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

        protected abstract bool IsContainedEdge(IEdge edge);
        
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
        
        public sealed class Ingoing : MemoryRelativeEdgeRepository
        {
            internal Ingoing(MemoryEdgeRepository edges, MemoryVertex vertex) : base(edges, vertex)
            {
            }

            public override IEdge Add(IVertex other, string label)
            {
                var edge = Edges.Create(other, Vertex, directed: true);
                edge.Label = label;
                return edge;
            }

            protected override bool IsContainedEdge(IEdge edge)
            {
                return edge.Directed && edge.ToVertex.Id == Vertex.Id;
            }
        }
        
        public sealed class Outgoing : MemoryRelativeEdgeRepository
        {
            internal Outgoing(MemoryEdgeRepository edges, MemoryVertex vertex) : base(edges, vertex)
            {
            }

            public override IEdge Add(IVertex other, string label)
            {
                var edge = Edges.Create(Vertex, other, directed: true);
                edge.Label = label;
                return edge;
            }

            protected override bool IsContainedEdge(IEdge edge)
            {
                return edge.Directed && edge.FromVertex.Id == Vertex.Id;
            }
        }
        
        public sealed class Bidirectional : MemoryRelativeEdgeRepository
        {
            internal Bidirectional(MemoryEdgeRepository edges, MemoryVertex vertex) : base(edges, vertex)
            {
            }

            public override IEdge Add(IVertex other, string label)
            {
                var edge = Edges.Create(Vertex, other, directed: false);
                edge.Label = label;
                return edge;
            }

            protected override bool IsContainedEdge(IEdge edge)
            {
                return !edge.Directed && (edge.FromVertex.Id == Vertex.Id || edge.ToVertex.Id == Vertex.Id);
            }
        }
    }
}