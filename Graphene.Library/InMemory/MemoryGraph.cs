using System.Collections.Generic;
using Graphene.InMemory.Query;
using Graphene.InMemory.Utility;
using Graphene.Query;

namespace Graphene.InMemory
{
    public class MemoryGraph : IGraph
    {
        public MemoryGraph()
        {
            MemoryEdges = new MemoryEdgeRepository(this);
            MemoryVertices = new MemoryVertexRepository(this, MemoryEdges);
            AvailableIds = new UniqueNumberSet(1, int.MaxValue);
        }

        public int Size => Vertices.Count() + Edges.Count();

        public IVertexRepository Vertices => MemoryVertices;

        public IReadOnlyRepository<IEdge> Edges => MemoryEdges;

        private MemoryVertexRepository MemoryVertices { get; }

        private MemoryEdgeRepository MemoryEdges { get; }

        private UniqueNumberSet AvailableIds { get; }

        public void Clear()
        {
            MemoryEdges.Clear();
            Vertices.Clear();
        }

        public IGraph Clone()
        {
            var result = new MemoryGraph();
            result.Merge(this);
            return result;
        }

        public void Merge(IGraph other)
        {
             var mappedIds = new Dictionary<int, int>();

            foreach (var vertex in other.Vertices)
            {
                var newVertex = Vertices.Create();
                mappedIds.Add(vertex.Id, newVertex.Id);
                newVertex.Label = vertex.Label;
                
                foreach (var attribute in vertex.Attributes)
                {
                    newVertex.Attributes.Set(attribute.Key, attribute.Value);
                }
            }

            foreach (var edge in other.Edges)
            {
                IEdge newEdge;
                var fromVertex = Vertices.Get(mappedIds[edge.FromVertex.Id]);
                var toVertex = Vertices.Get(mappedIds[edge.ToVertex.Id]);

                if (edge.Directed)
                    newEdge = fromVertex.OutgoingEdges.Add(toVertex);
                else
                    newEdge = fromVertex.BidirectionalEdges.Add(toVertex);

                newEdge.Label = edge.Label;

                foreach (var attribute in edge.Attributes)
                {
                    newEdge.Attributes.Set(attribute.Key, attribute.Value);
                }
            }
        }

        public IQueryBuilderRoot Select()
        {
            return new BuilderRoot(this);
        }

        internal int TakeId()
        {
            return AvailableIds.SampleRandom();
        }

        internal void FreeId(int id)
        {
            AvailableIds.Add(id);
        }
    }
}