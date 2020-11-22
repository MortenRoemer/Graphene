using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query.Entity;

namespace Graphene.InMemory.Query.Entity
{
    public class Vertices : IVertices
    {
        public Vertices(IReadOnlyGraph graph, IEnumerable<int> range)
        {
            Graph = graph;
            Range = range;
        }
        
        private IReadOnlyGraph Graph { get; }
        
        private IEnumerable<int> Range { get; }
        
        private Func<IReadOnlyVertex, bool> Filter { get; set; }
        
        public IVertices Where(Func<IReadOnlyVertex, bool> filter)
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));
            
            Filter = Filter is null
                ? filter
                : vertex => Filter(vertex) && filter(vertex);

            return this;
        }

        public bool Resolve(out IPagedResult<IReadOnlyVertex> pagedResult)
        {
            return Resolve(VertexPagedResult.DEFAULT_PAGE_SIZE, 0, out pagedResult);
        }

        public bool Resolve(int pageSize, out IPagedResult<IReadOnlyVertex> pagedResult)
        {
            if (pageSize <= 0)
                throw new ArgumentException($"pageSize has to be greater than zero. given: {pageSize}");
            
            return Resolve(pageSize, 0, out pagedResult);
        }

        internal bool Resolve(int pageSize, int offset, out IPagedResult<IReadOnlyVertex> pagedResult)
        {
            var vertexSource = Range is null
                ? Graph.Vertices
                : Graph.Vertices.Get(Range);

            vertexSource = vertexSource.SkipWhile(vertex => vertex.Id <= offset);

            if (Filter != null)
                vertexSource = vertexSource.Where(Filter);

            var entities = vertexSource.Take(pageSize).ToArray();
            pagedResult = new VertexPagedResult(this, entities, pageSize);
            return entities.Length > 0;
        }
    }
}