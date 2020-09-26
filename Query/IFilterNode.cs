namespace Graphene.Query
{
    public interface IFilterNode<T>
    {
        IFilterAttribute<T> Attribute(string name);

        IFilterLabel<T> Label();
    }
}