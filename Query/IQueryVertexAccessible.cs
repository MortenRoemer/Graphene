using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryVertexAccessible
    {
        IQueryBuilderVertex AnyVertex();

        IQueryBuilderVertex Vertex(long id);

        IQueryBuilderVertex Vertices(IEnumerable<long> ids);
    }
}