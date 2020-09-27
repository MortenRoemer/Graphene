namespace Graphene.Query
{
    public interface IGroupFilterRoot<T> : IGroupFilterNode<T>
    {
        T EndWhere();
    }
}