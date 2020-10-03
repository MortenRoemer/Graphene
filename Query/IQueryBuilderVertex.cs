using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryBuilderVertex : IQueryBuilder, IQueryFilterable<IQueryBuilderVertex>, IQueryRoutable
    {
        IQueryBuilderEdge AnyEdges();

        IQueryBuilderEdge AnyIngoingEdges();

        IQueryBuilderEdge AnyOutgoingEdges();

        IQueryBuilderEdge Edge(long id);

        IQueryBuilderEdge Edges(IEnumerable<long> ids);
    }
}