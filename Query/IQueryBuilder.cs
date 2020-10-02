namespace Graphene.Query
{
    public interface IQueryBuilder
    {
        IQueryResult Resolve();
    }
}