namespace Graphene.Query
{
    public interface IGroupFilterNode<T>
    {
        IGroupFilterAttribute<T> Attribute(string name);

        IGroupFilterLabel<T> Label();

        IGroupFilterAggregate<T> TheAverageOf();

        IGroupFilterAggregate<T> TheMaximumOf();

        IGroupFilterAggregate<T> TheMinimumOf();

        IGroupFilterAggregate<T> TheSumOf();
    }
}