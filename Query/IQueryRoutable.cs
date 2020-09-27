using System;
using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryRoutable
    {
        IQueryBuilderRoute RouteToAnyEdge();

        IQueryBuilderRoute RouteToEdge(Guid id);

        IQueryBuilderRoute RouteToEdges(IEnumerable<Guid> ids);

        IQueryBuilderRoute RouteToAnyVertex();

        IQueryBuilderRoute RouteToVertex(Guid id);

        IQueryBuilderRoute RouteToVertices(IEnumerable<Guid> ids);
    }
}
