using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query.SubGraph;

namespace Graphene.InMemory.Query.SubGraph
{
    public class VerticesWithEdges : IVerticesWithEdges
    {
        internal VerticesWithEdges(Vertices vertices, IEnumerable<int> range)
        {
            Vertices = vertices;
            Range = range;
        }
        
        private Vertices Vertices { get; }
        
        private IEnumerable<int> Range { get; }
        
        private Func<IReadOnlyEdge, bool> Filter { get; set; }
        
        public IReadOnlyGraph Resolve()
        {
            var vertexSource = Vertices.Range is null
                ? Vertices.Root.Graph.Vertices
                : Vertices.Root.Graph.Vertices.Get(Range);

            if (Vertices.Filter != null)
                vertexSource = vertexSource.Where(Vertices.Filter);
            
            var vertexRange = new HashSet<int>(vertexSource.Select(vertex => vertex.Id));

            var edgeSource = Range is null
                ? Vertices.Root.Graph.Edges
                : Vertices.Root.Graph.Edges.Get(Range);

            if (Filter != null)
                edgeSource = edgeSource.Where(Filter);

            edgeSource = edgeSource.Where(edge =>
                vertexRange.Contains(edge.FromVertex.Id) || vertexRange.Contains(edge.ToVertex.Id));
            
            var edgeRange = new HashSet<int>(edgeSource.Select(edge => edge.Id));
            return new MemoryGraphView(Vertices.Root.Graph, vertexRange, edgeRange);
        }

        public IVerticesWithEdges Where(Func<IReadOnlyEdge, bool> filter)
        {
            Filter = Filter is null ? filter : edge => Filter(edge) && filter(edge);
            return this;
        }
    }
}