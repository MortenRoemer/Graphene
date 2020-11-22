using System;

namespace Graphene.Query.Entity
{
    public interface IEdges
    {
        IEdges Where(Func<IReadOnlyEdge, bool> filter);
        
        bool Resolve(out IPagedResult<IReadOnlyEdge> pagedResult);

        bool Resolve(int pageSize, out IPagedResult<IReadOnlyEdge> pagedResult);
    }
}