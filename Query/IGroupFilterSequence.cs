namespace Graphene.Query
{
    public interface IGroupFilterSequence<T> : IGroupFilterRoot<T>
    {
        IGroupFilterNode<T> And();

        IGroupFilterNode<T> Or();

        IGroupFilterNode<T> Xor();
    }
}