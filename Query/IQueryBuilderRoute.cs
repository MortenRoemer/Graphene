namespace Graphene.Query
{
    public interface IQueryBuilderRoute : IQueryBuilder, IQueryFilterable<IQueryBuilderRoute>
    {
        IQueryBuilderOptimizer<IQueryBuilderRoute> OptimizeSoThat();

        IFilterRoot<IQueryBuilderRoute> WhereAnyHopEdge();

        IFilterRoot<IQueryBuilderRoute> WhereAnyHopVertex();

        IQueryBuilderRoute WithEdgeHopLimit(long limit);

        IQueryBuilderRoute WithVertexHopLimit(long limit);
    }
}