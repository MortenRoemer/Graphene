namespace Graphene.Query
{
    public interface IQueryBuilder
    {
        bool Resolve(out IQueryResult result);
    }
}