using System;
using System.Collections.Generic;
using System.Linq;
using Graphene.Query.Entity;

namespace Graphene.InMemory.Query.Entity
{
    public class Edges : IEdges
    {
        internal Edges(IReadOnlyGraph graph, IEnumerable<int>? range)
        {
            Graph = graph;
            Range = range;
        }
        
        private IReadOnlyGraph Graph { get; }
        
        private IEnumerable<int>? Range { get; }
        
        private Func<IReadOnlyEdge, bool>? Filter { get; set; }
        
        public IEdges Where(Func<IReadOnlyEdge, bool> filter)
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));
            
            Filter = Filter is null
                ? filter
                : edge => Filter(edge) && filter(edge);

            return this;
        }

        public bool Resolve(out IPagedResult<IReadOnlyEdge> pagedResult)
        {
            return Resolve(EdgePagedResult.DEFAULT_PAGE_SIZE, 0, out pagedResult);
        }

        public bool Resolve(int pageSize, out IPagedResult<IReadOnlyEdge> pagedResult)
        {
            if (pageSize <= 0)
                throw new ArgumentException($"pageSize has to be greater than zero. given: {pageSize}");
            
            return Resolve(pageSize, 0, out pagedResult);
        }

        internal bool Resolve(int pageSize, int offset, out IPagedResult<IReadOnlyEdge> pagedResult)
        {
            var edgeSource = Range is null
                ? Graph.Edges
                : Graph.Edges.Get(Range);

            edgeSource = edgeSource.SkipWhile(edge => edge.Id <= offset);

            if (Filter is not null)
                edgeSource = edgeSource.Where(Filter);

            var entities = edgeSource.Take(pageSize).ToArray();
            pagedResult = new EdgePagedResult(this, entities, pageSize);
            return entities.Length > 0;
        }
    }
}