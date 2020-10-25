using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryRoutable
    {
        IQueryBuilderRoute RouteToAnyVertex();

        IQueryBuilderRoute RouteToVertex(ulong id);

        IQueryBuilderRoute RouteToVertices(IEnumerable<ulong> ids);
    }
}
