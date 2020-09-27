using System;
using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryBuilderVertex : IQueryBuilder, IQueryFilterable<IQueryBuilderVertex>, IQueryRoutable
    {
        IQueryBuilderEdge AnyEdges();

        IQueryBuilderEdge AnyIngoingEdges();

        IQueryBuilderEdge AnyOutgoingEdges();

        IQueryBuilderEdge Edge(Guid id);

        IQueryBuilderEdge Edges(IEnumerable<Guid> ids);
    }
}