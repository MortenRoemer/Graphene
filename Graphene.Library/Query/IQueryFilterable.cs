namespace Graphene.Query
{
    public interface IQueryFilterable<T>
    {
        IFilterRoot<T> Where();
    }
}