using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryRoutable
    {
        IQueryBuilderRoute RouteToAnyEdge();

        IQueryBuilderRoute RouteToEdge(long id);

        IQueryBuilderRoute RouteToEdges(IEnumerable<long> ids);

        IQueryBuilderRoute RouteToAnyVertex();

        IQueryBuilderRoute RouteToVertex(long id);

        IQueryBuilderRoute RouteToVertices(IEnumerable<long> ids);
    }
}
