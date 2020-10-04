using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryBuilderEdge : IQueryBuilder, IQueryFilterable<IQueryBuilderEdge>
    {
        IQueryBuilderVertex AnyVertices();

        IQueryBuilderVertex SourceVertex();

        IQueryBuilderVertex TargetVertex();

        IQueryBuilderVertex Vertex(ulong id);

        IQueryBuilderVertex Vertices(IEnumerable<ulong> ids);
    }
}