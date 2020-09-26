using System;
using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryEdgeAccessible
    {
        IQueryBuilderEdge AnyEdges();

        IQueryBuilderEdge Edge(Guid id);

        IQueryBuilderEdge Edges(IEnumerable<Guid> ids);
    }
}