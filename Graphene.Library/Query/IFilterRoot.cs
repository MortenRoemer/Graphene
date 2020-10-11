namespace Graphene.Query
{
    public interface IFilterRoot<T> : IFilterNode<T>
    {
        T EndWhere();
    }
}