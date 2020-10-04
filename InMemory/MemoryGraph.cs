using System.Collections.Generic;
using Graphene.InMemory.Query;
using Graphene.Query;

namespace Graphene.InMemory
{
    public class MemoryGraph : IGraph
    {
        public MemoryGraph()
        {
            MemoryEdges = new MemoryEdgeRepository(this);
            MemoryVertices = new MemoryVertexRepository(this, MemoryEdges);
        }

        public long Size => Vertices.Count() + Edges.Count();

        public IVertexRepository Vertices => MemoryVertices;

        public IReadOnlyRepository<IEdge> Edges => MemoryEdges;

        private MemoryVertexRepository MemoryVertices { get; }

        private MemoryEdgeRepository MemoryEdges { get; }

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
             var mappedIds = new Dictionary<ulong, ulong>();

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
                var ToVertex = Vertices.Get(mappedIds[edge.ToVertex.Id]);

                if (edge.Directed)
                    newEdge = fromVertex.OutgoingEdges.Add(ToVertex);
                else
                    newEdge = fromVertex.BidirectionalEdges.Add(ToVertex);

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
    }
}