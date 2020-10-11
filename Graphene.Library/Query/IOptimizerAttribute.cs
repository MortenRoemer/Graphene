namespace Graphene.Query
{
    public interface IOptimizerAttribute<T>
    {
        T IsMaximal();

        T IsMinimal();
    }
}