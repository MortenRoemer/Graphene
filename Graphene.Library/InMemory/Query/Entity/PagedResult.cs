using System.Collections.Generic;
using System.Linq;
using Graphene.Query.Entity;

namespace Graphene.InMemory.Query.Entity
{
    public abstract class PagedResult<TEntity> : IPagedResult<TEntity>
    {
        internal const int DEFAULT_PAGE_SIZE = 50;
        
        protected PagedResult(IEnumerable<TEntity> entities, int pageSize)
        {
            Entities = entities is IReadOnlyList<TEntity> list ? list : entities.ToArray();
            PageSize = pageSize;
        }

        public int PageSize { get; }

        public IReadOnlyList<TEntity> Entities { get; }

        public abstract bool FindNextPage(out IPagedResult<TEntity> nextPage);
    }
}