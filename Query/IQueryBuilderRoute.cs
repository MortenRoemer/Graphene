namespace Graphene.Query
{
    public interface IQueryBuilderRoute : IQueryBuilder, IQueryFilterable<IQueryBuilderRoute>
    {
        IFilterRoot<IQueryBuilderRoute> WhereAnyHopEdge();

        IFilterRoot<IQueryBuilderRoute> WhereAnyHopVertex();

        IQueryBuilderRoute WithEdgeHopLimit(long limit);

        IQueryBuilderRoute WithVertexHopLimit(long limit);
    }
}