using System;
using System.Collections.Generic;

namespace Graphene.Query
{
    public interface IQueryBuilderVertex : IQueryBuilder, IQueryFilterable<IQueryBuilderVertex>
    {
        IQueryBuilderEdge AnyEdges();

        IQueryBuilderEdge AnyBidrectionalEdges();

        IQueryBuilderEdge AnyIngoingEdges();

        IQueryBuilderEdge AnyOutgoingEdges();

        IQueryBuilderEdge Edge(Guid id);

        IQueryBuilderEdge Edges(IEnumerable<Guid> id);
    }
}