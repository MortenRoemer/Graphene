using System.Collections.Generic;
using Graphene.Query.Entity;

namespace Graphene.InMemory.Query.Entity
{
    public class VertexPagedResult : PagedResult<IReadOnlyVertex>
    {
        internal VertexPagedResult(Vertices query, IEnumerable<IReadOnlyVertex> entities, int pageSize) : base(entities, pageSize)
        {
            Query = query;
        }

        private Vertices Query { get; }

        public override bool FindNextPage(out IPagedResult<IReadOnlyVertex> nextPage)
        {
            if (Entities.Count <= 0)
            {
                nextPage = default;
                return false;
            }
            
            return Query.Resolve(PageSize, Entities[Entities.Count - 1].Id, out nextPage);
        }
    }
}