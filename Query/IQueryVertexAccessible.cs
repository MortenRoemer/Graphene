using System;
using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryVertexAccessible
    {
        IQueryBuilderVertex AnyVertex();

        IQueryBuilderVertex Vertex(Guid id);

        IQueryBuilderVertex Vertices(IEnumerable<Guid> ids);
    }
}