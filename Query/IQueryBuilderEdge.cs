using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryBuilderEdge : IQueryBuilder, IQueryFilterable<IQueryBuilderEdge>
    {
        IQueryBuilderVertex AnyVertices();

        IQueryBuilderVertex SourceVertex();

        IQueryBuilderVertex TargetVertex();

        IQueryBuilderVertex Vertex(long id);

        IQueryBuilderVertex Vertices(IEnumerable<long> ids);
    }
}