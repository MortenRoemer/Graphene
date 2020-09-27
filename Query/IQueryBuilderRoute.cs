namespace Graphene.Query
{
    public interface IQueryBuilderRoute : IQueryBuilder, IQueryFilterable<IQueryBuilderRoute>
    {
        IGroupFilterRoot<IQueryBuilderRoute> WhereAnyHopEdge();

        IGroupFilterRoot<IQueryBuilderRoute> WhereAnyHopVertex();

        IQueryBuilderRoute WithEdgeHopLimit(long limit);

        IQueryBuilderRoute WithVertexHopLimit(long limit);
    }
}