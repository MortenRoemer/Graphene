using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query.SubGraph;

namespace Graphene.InMemory.Query.SubGraph
{
    public class Vertices : IVertices
    {
        internal Vertices(Root root, IEnumerable<int> range)
        {
            Root = root;
            Range = range;
        }
        
        internal Root Root { get; }
        
        internal IEnumerable<int> Range { get; }
        
        internal Func<IReadOnlyVertex, bool> Filter { get; private set; }
        
        public IReadOnlyGraph Resolve()
        {
            IEnumerable<IReadOnlyVertex> vertexRange = Range is null
                ? Root.Graph.Vertices
                : Root.Graph.Vertices.Get(Range);

            if (Filter != null)
                vertexRange = vertexRange.Where(Filter);

            return new MemoryGraphView(Root.Graph, vertexRange.Select(vertex => vertex.Id), Enumerable.Empty<int>());
        }

        public IVertices Where(Func<IReadOnlyVertex, bool> filter)
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));
            
            Filter = Filter is null ? filter : vertex => Filter(vertex) && filter(vertex);
            return this;
        }

        public IVerticesWithEdges WithEdges()
        {
            return new VerticesWithEdges(this, null);
        }

        public IVerticesWithEdges WithEdges(IEnumerable<int> ids)
        {
            if (ids is null)
                throw new ArgumentNullException(nameof(ids));
            
            return new VerticesWithEdges(this, ids);
        }
    }
}