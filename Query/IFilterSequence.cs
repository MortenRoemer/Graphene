namespace Graphene.Query
{
    public interface IFilterSequence<T> : IFilterRoot<T>
    {
        IFilterNode<T> And();

        IFilterNode<T> Or();

        IFilterNode<T> Xor();
    }
}