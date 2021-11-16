using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Graphene.InMemory
{
    public class FindResult<T> : IFindResult<T> where T : IReadOnlyEntity
    {
        public FindResult(
            MemoryGraph graph,
            Expression<Func<T, bool>>? filter,
            int pageSize,
            bool moreResults,
            IReadOnlyCollection<T> results)
        {
            Graph = graph;
            Filter = filter;
            PageSize = pageSize;
            MoreResults = moreResults;
            Results = results;
        }

        private MemoryGraph Graph { get; }

        private Expression<Func<T, bool>>? Filter { get; }

        public int PageSize { get; }
        
        public bool MoreResults { get; }
        
        public IReadOnlyCollection<T> Results { get; }
        
        public async Task<IFindResult<T>> GetMoreResults()
        {
            if (!MoreResults)
                throw new InvalidOperationException("There are no more records to find for this query");
            
            return await Task.Run(() => Graph.Find(PageSize, Results.Last().Id, Filter));
        }
    }
}