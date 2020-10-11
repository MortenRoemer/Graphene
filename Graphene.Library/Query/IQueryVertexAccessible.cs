using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryVertexAccessible
    {
        IQueryBuilderVertex AnyVertex();

        IQueryBuilderVertex Vertex(ulong id);

        IQueryBuilderVertex Vertices(IEnumerable<ulong> ids);
    }
}