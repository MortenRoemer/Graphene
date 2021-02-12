using System.Collections.Generic;
using Graphene.Query.Entity;

namespace Graphene.InMemory.Query.Entity
{
    public class EdgePagedResult : PagedResult<IReadOnlyEdge>
    {
        public EdgePagedResult(Edges query, IEnumerable<IReadOnlyEdge> entities, int pageSize) : base(entities, pageSize)
        {
            Query = query;
        }
        
        private Edges Query { get; }

        public override bool FindNextPage(out IPagedResult<IReadOnlyEdge> nextPage)
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