using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryRoutable
    {
        IQueryBuilderRoute RouteToAnyEdge();

        IQueryBuilderRoute RouteToEdge(ulong id);

        IQueryBuilderRoute RouteToEdges(IEnumerable<ulong> ids);

        IQueryBuilderRoute RouteToAnyVertex();

        IQueryBuilderRoute RouteToVertex(ulong id);

        IQueryBuilderRoute RouteToVertices(IEnumerable<ulong> ids);
    }
}
