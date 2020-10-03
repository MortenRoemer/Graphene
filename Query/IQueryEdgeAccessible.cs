using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryEdgeAccessible
    {
        IQueryBuilderEdge AnyEdges();

        IQueryBuilderEdge Edge(long id);

        IQueryBuilderEdge Edges(IEnumerable<long> ids);
    }
}