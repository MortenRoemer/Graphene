using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query.SubGraph;

namespace Graphene.InMemory.Query.SubGraph
{
    public class Edges : IEdges
    {
        internal Edges(Root root, IEnumerable<int>? range)
        {
            Root = root;
            Range = range;
        }

        private Root Root { get; }

        private IEnumerable<int>? Range { get; }

        private Func<IReadOnlyEdge, bool> Filter { get; set; }
        
        public IReadOnlyGraph Resolve()
        {
            var edgeSource = Range is null
                ? Root.Graph.Edges
                : Root.Graph.Edges.Get(Range);

            if (Filter != null)
                edgeSource = edgeSource.Where(Filter);
            
            var vertexRange = new HashSet<int>();
            var edgeRange = new HashSet<int>();

            foreach (var edge in edgeSource)
            {
                vertexRange.Add(edge.FromVertex.Id);
                vertexRange.Add(edge.ToVertex.Id);
                edgeRange.Add(edge.Id);
            }
            
            return new MemoryGraphView(Root.Graph, vertexRange, edgeRange);
        }

        public IEdges Where(Func<IReadOnlyEdge, bool> filter)
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));
            
            Filter = Filter is null ? filter : edge => Filter(edge) && filter(edge);
            return this;
        }
    }
}