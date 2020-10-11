namespace Graphene.Query
{
    public interface IQueryBuilderRoute : IQueryBuilder, IQueryFilterable<IQueryBuilderRoute>
    {
        IQueryBuilderOptimizer<IQueryBuilderRoute> OptimizeSoThat();

        IFilterRoot<IQueryBuilderRoute> WhereAnyHopEdge();

        IFilterRoot<IQueryBuilderRoute> WhereAnyHopVertex();

        IQueryBuilderRoute WithEdgeHopLimit(ulong limit);

        IQueryBuilderRoute WithVertexHopLimit(ulong limit);
    }
}