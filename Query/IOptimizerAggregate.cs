namespace Graphene.Query
{
    public interface IOptimizerAggregate<T>
    {
        IOptimizerAttribute<T> Attribute(string name);
    }
}