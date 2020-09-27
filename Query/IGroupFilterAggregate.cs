namespace Graphene.Query
{
    public interface IGroupFilterAggregate<T>
    {
        IGroupFilterAttribute<T> Attribute(string name);
    }
}