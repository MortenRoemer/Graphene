using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryEdgeAccessible
    {
        IQueryBuilderEdge AnyEdges();

        IQueryBuilderEdge Edge(ulong id);

        IQueryBuilderEdge Edges(IEnumerable<ulong> ids);
    }
}