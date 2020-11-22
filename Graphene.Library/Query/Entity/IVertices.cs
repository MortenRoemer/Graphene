using System;

namespace Graphene.Query.Entity
{
    public interface IVertices
    {
        IVertices Where(Func<IReadOnlyVertex, bool> filter);

        bool Resolve(out IPagedResult<IReadOnlyVertex> pagedResult);

        bool Resolve(int pageSize, out IPagedResult<IReadOnlyVertex> pagedResult);
    }
}