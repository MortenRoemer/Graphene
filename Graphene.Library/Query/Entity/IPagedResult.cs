using System.Collections.Generic;

namespace Graphene.Query.Entity
{
    public interface IPagedResult<TEntity>
    {
        int PageSize { get; }
        
        IReadOnlyList<TEntity> Entities { get; }

        bool FindNextPage(out IPagedResult<TEntity> nextPage);
    }
}